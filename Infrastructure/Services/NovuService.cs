using Novu;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BeebopNoteApp.Domain.Common;
using BeebopNoteApp.Domain.Interfaces;
using BeebopNoteApp.Domain.Models;

namespace BeebopNoteApp.Infrastructure.Services;

public class NovuService : IDisposable
{
    private readonly NovuSDK _novuSdk;
    public string _subscriberId = string.Empty;
    public string _applicationId = string.Empty;

    private readonly IAppLogger _logger;
    private readonly HttpClient _httpClient;
    private readonly IAppConfig _config;
    private readonly IRepository _repository;
    private readonly IAppNotification _appNotification;

    private Timer? _notificationTimer;
    private bool _isProcessingNotifications = false;
    private bool _disposed = false;

    public event EventHandler<AppNotificationModel>? NewNotificationReceived;

    public NovuService(
        IAppConfig config,
        IRepository repository,
        NovuSDK novuSdk,
        IAppNotification appNotification,
        IAppLogger logger)
    {
        _config = config;
        _repository = repository;
        _novuSdk = novuSdk;
        _appNotification = appNotification;
        _logger = logger;

        _httpClient = new HttpClient()
        {
            BaseAddress = new(_config.NovuUrl),
        };

        _httpClient.DefaultRequestHeaders.Authorization = new("ApiKey", $"{_config.NovuApiKey}");

        _logger.Debug<NovuService>("NovuService initialized... waiting for notifications");
    }

    public async Task Start()
    {
        try
        {
            _logger.Info<NovuService>("NovuService starting...");
            await RegisterWithNovuAsync();

            _logger.Info<NovuService>("Registration with Novu complete, starting notification listener.");
            StartListeningForNotifications();
            _logger.Info<NovuService>("Notification listener started.");
        }
        catch (Exception ex)
        {
            _logger.Error<NovuService>($"Exception in NovuService.Start(): {ex}");
        }
    }

    public async Task RegisterWithNovuAsync()
    {
        _logger.Debug<NovuService>("Registering with Novu Cloud.");
        try
        {
            if (string.IsNullOrWhiteSpace(_config.NovuApiKey) || string.IsNullOrWhiteSpace(_config.NovuUrl))
            {
                _logger.Error<NovuService>("Novu API key or URL is missing. Registration aborted.");
                return;
            }

            string? subscriberId = await _repository.GetSubscriberId();
            if (string.IsNullOrEmpty(subscriberId))
            {
                _logger.Debug<NovuService>("No SubscriberID is set; registering a new subscriber with Novu.");
                _subscriberId = ShortGuid.ToShortGuid(Guid.NewGuid());
                _logger.Debug<NovuService>($"Generated new Subscriber ID: {_subscriberId}");

                Novu.Models.Components.CreateSubscriberRequestDto createRequest = new()
                {
                    FirstName = _config.NovuFirstName ?? Environment.MachineName,
                    LastName = _config.NovuLastName ?? "Notifier",
                    Email = _config.NovuEmail ?? "novu@novu.de",
                    SubscriberId = _subscriberId
                };

                try
                {
                    _logger.Debug<NovuService>($"Creating Novu subscriber: {createRequest.Email}, {createRequest.SubscriberId}");
                    await _novuSdk.Subscribers.CreateAsync(createRequest);
                    _logger.Debug<NovuService>("Successfully registered with Novu.");

                    // Asynchronously save the new ID to the database
                    await _repository.SetSubscriberId(_subscriberId);
                }
                catch (Novu.Models.Errors.ErrorDto ex) when (ex.Message?.Contains("already exists", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    _logger.Info<NovuService>("Subscriber already exists in Novu. Continuing.");
                }
                catch (Exception ex)
                {
                    _logger.Error<NovuService>($"Error during Novu registration process: {ex.Message}", ex);
                    return; // Abort on failure
                }
            }
            else
            {
                _subscriberId = subscriberId;
            }
            _logger.Debug<NovuService>($"Using Subscriber ID: {_subscriberId}");
        }
        catch (Exception ex)
        {
            _logger.Error<NovuService>($"Unexpected error during Novu registration: {ex.Message}", ex);
            throw;
        }
    }

    public async Task UpdateNovuSubscriberAsync(
        string subscriberId,
        string firstName,
        string lastName,
        string email)
    {
        _logger.Debug<NovuService>($"Attempting to update Novu subscriber: {subscriberId}");

        if (string.IsNullOrWhiteSpace(subscriberId))
        {
            _logger.Error<NovuService>("Cannot update subscriber without a valid Subscriber ID.");
            return;
        }

        try
        {
            Novu.Models.Components.PatchSubscriberRequestDto updateRequest = new()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            };

            Novu.Models.Requests.SubscribersControllerPatchSubscriberResponse result = await _novuSdk.Subscribers.PatchAsync(subscriberId, updateRequest);

            if (result.HttpMeta.Response.IsSuccessStatusCode)
            {
                _logger.Debug<NovuService>($"Successfully updated subscriber {subscriberId}.");
            }
            else
            {
                _logger.Error<NovuService>($"Failed to update subscriber {subscriberId}. Status: {result.HttpMeta.Response.StatusCode}, Message: {result.HttpMeta.Response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error<NovuService>($"Error updating Novu subscriber {subscriberId}: {ex.Message}", ex);
        }
    }

    public void StartListeningForNotifications()
    {
        _logger.Debug<NovuService>("Starting notification listener timer.");
        _notificationTimer = new Timer(
            async _ => await CheckForNotificationsAsync(),
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(10));
    }

    private async Task CheckForNotificationsAsync()
    {
        if (_isProcessingNotifications)
        {
            return;
        }

        _isProcessingNotifications = true;

        try
        {
            _logger.Debug<NovuService>("Fetching notifications from Novu feed...");

            HttpResponseMessage httpResponse = await _httpClient.GetAsync(
                $"v1/subscribers/{_subscriberId}/notifications/feed");
            _logger.Debug<NovuService>(" base address = " + _httpClient.BaseAddress.ToString() + "v1/subscribers/" + _subscriberId + "/notifications/feed" );

            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.Warn<NovuService>(await httpResponse.Content.ReadAsStringAsync());
            }

            NovuNotification? response = await httpResponse.Content.ReadFromJsonAsync<NovuNotification>();

            if (response?.Data == null)
            {
                _logger.Debug<NovuService>("No new notifications or failed to parse response.");
                return;
            }

            _logger.Debug<NovuService>($"Received {response.Data.Length} notifications from feed.");
            foreach (Datum? d in response.Data.OrderBy(n => n.CreatedAt)) // Process in chronological order
            {
                // To prevent re-notifying, check if we already have this in our local DB
                AppNotificationModel? existingNotification = await _repository.GetNotificationById(d.Id);
                bool isNewNotification = existingNotification == null;

                AppNotificationModel notification = new()
                {
                    Id = d.Id,
                    Title = d.TemplateIdentifier ?? "<NULL>",
                    Message = d.Content ?? "<NULL>",
                    Subscriberid = d.Subscriber.SubscriberId,
                    ReceivedDate = isNewNotification ? DateTime.Now : existingNotification!.ReceivedDate,
                    IsRead = d.Read,
                    Subject = d.Subject ?? "<NULL>",
                    Priority = d.Data?.Priority ?? BeebopNoteApp.Domain.Models.Constants.PriorityLevels.Normal
                };

                // Save to database (this is an upsert, so it's safe to call every time)
                await _repository.SaveNotification(notification);

                if (isNewNotification)
                {
                    _logger.Debug<NovuService>($"New notification detected: {notification.Id} - {notification.Title}");
                    await ShowNativeNotification(notification);

                    NewNotificationReceived?.Invoke(this, notification); // Notify subscribers
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error<NovuService>($"Error checking for notifications: {ex.Message}", ex);
        }
        finally
        {
            _isProcessingNotifications = false;
        }
    }

    private async Task ShowNativeNotification(AppNotificationModel notification)
    {
        try
        {
            _logger.Debug<NovuService>($"Requesting native notification for: {notification.Title}");
            await _appNotification.Show(notification);
            _logger.Debug<NovuService>($"Native notification shown successfully for: {notification.Title}");
        }
        catch (Exception ex)
        {
            _logger.Error<NovuService>($"Failed to show native notification: {ex.Message}", ex);
        }
    }

    public async Task MarkMessageAsAsync(string messageId, Novu.Models.Components.MarkAs markAs)
    {
        string? subscriberId = await _repository.GetSubscriberId();

        if (string.IsNullOrWhiteSpace(subscriberId))
        {
            _logger.Debug<NovuService>($"No SubscriberID is set; cannot mark message {messageId} as {markAs}.");
        }
        else
        {
            var payload = new
            {
                messageId,
                markAs = markAs.ToString().ToLower()
            };

            JsonContent content = JsonContent.Create(payload);
            HttpResponseMessage response = await _httpClient.PostAsync($"v1/subscribers/{subscriberId}/messages/mark-as", content);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                _logger.Error<NovuService>($"Failed to mark message {messageId} as {markAs}. Status: {response.StatusCode}, Error: {error}");
            }
        }

    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _logger?.Debug<NovuService>("Disposing NovuService");
                _notificationTimer?.Dispose();
                _httpClient?.Dispose();
            }
            _disposed = true;
        }
    }

    ~NovuService()
    {
        Dispose(false);
    }
}