using log4net;
using System;
using System.Threading.Tasks;
using BeebopNoteApp.Domain.Interfaces;
using BeebopNoteApp.Domain.Models;
using BeebopNoteApp.Infrastructure.Services;

namespace BeebopNoteApp.Views.UrgentMessage;

public partial class UrgentMessageViewModel
{
    private readonly IAppLogger _logger;
    private readonly IRepository _repository;
    private readonly AppNotificationModel _notification;
    private readonly Window _owner;
    private readonly NovuService _novuService;

    public string Id
    {
        get => _notification.Id;
        set => _notification.Id = value;
    }

    public string SubscriberId
    {
        get => _notification.Subscriberid;
        set => _notification.Subscriberid = value;
    }

    private string _title;
    private string _message;
    private DateTime _receivedDate;
    private bool _isRead;
    private string _subject;
    private string _priority;

    // Primary constructor for dependency injection
    public UrgentMessageViewModel(
        Window owner,
        AppNotificationModel notification,
        IRepository repository,
        NovuService novuService,
        IAppLogger logger)
    {
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));
        _notification = notification ?? throw new ArgumentNullException(nameof(notification));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _novuService = novuService ?? throw new ArgumentNullException(nameof(novuService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _title = notification.Title;
        _message = notification.Message;
        _receivedDate = notification.ReceivedDate;
        _isRead = notification.IsRead;
        _subject = notification.Subject;
        _priority = notification.Priority;

        _logger.Debug<UrgentMessageViewModel>($"UrgentMessageViewModel created with message: {_notification.Message}");
    }
    /*
    public async Task AcknowledgeButtonClicked()
    {
        if (!IsRead)
        {
            IsRead = true;
            try
            {
                _logger.Debug<UrgentMessageViewModel>($"Updating notification as read: {_notification.Id}");
                await _repository.MarkAsRead(_notification.Id);
                await _novuService.MarkMessageAsAsync(_notification.Id, Novu.Models.Components.MarkAs.Read);
                _owner.Close();
            }
            catch (Exception ex)
            {
                _logger.Error<UrgentMessageViewModel>($"Failed to save notification: {ex.Message}");
                throw;
            }
        }
    }
    */
    /*
    protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(Title)) _notification.Title = Title;
        if (e.PropertyName == nameof(Message)) _notification.Message = Message;
        if (e.PropertyName == nameof(ReceivedDate)) _notification.ReceivedDate = ReceivedDate;
        if (e.PropertyName == nameof(IsRead)) _notification.IsRead = IsRead;
        if (e.PropertyName == nameof(Subject)) _notification.Subject = Subject;
        if (e.PropertyName == nameof(Priority)) _notification.Priority = Priority;
    }
    */

}