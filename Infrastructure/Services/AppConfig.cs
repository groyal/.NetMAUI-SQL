using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using BeebopNoteApp.Domain.Interfaces;
using System.Text.Json;

namespace BeebopNoteApp.Infrastructure.Services;

public class AppConfig(IAppLogger logger) : IAppConfig
{
    private readonly IAppLogger _logger = logger;

    public string DbConnectionString => "";   //$"Data Source={ApplicationResources.DBFileName};";
    public string NovuApiKey => GetConfigValue("NovuApiKey", "Novu API key is not configured.");
    public string NovuUrl => GetConfigValue("NovuUrl", "Novu URL is not configured.");
    public string NovuSubscriberId => ConfigurationManager.AppSettings["NovuSubscriberId"] ?? string.Empty;
    public string NovuApplicationId => ConfigurationManager.AppSettings["NovuApplicationId"] ?? string.Empty;
    public string NovuSubscriberPrefix => ConfigurationManager.AppSettings["NovuSubscriberPrefix"] ?? string.Empty;
    public string NovuFirstName => ConfigurationManager.AppSettings["NovuFirstName"] ?? Environment.MachineName;
    public string NovuLastName => ConfigurationManager.AppSettings["NovuLastName"] ?? "Notifier";
    public string NovuEmail => ConfigurationManager.AppSettings["NovuEmail"] ?? "novu@novu.de";
    public AppSettings Settings => LoadFromEmbeddedJson();

    private string GetConfigValue(string key, string errorMessage)
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                string? value = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrWhiteSpace(value))
                {
                    _logger.Error<AppConfig>(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }
                return value;
            }
            else if (OperatingSystem.IsAndroid())
            {
                string? value = string.Empty;

                switch (key)
                {
                    case "DbConnectionString":
                        value = string.Empty;
                        break;
                    case "NovuApiKey":
                        value = Settings.NovuApiKey;
                        break;
                    case "NovuUrl":
                        value = Settings.NovuUrl;
                        break;
                    case "NovuSubscriberId":
                        value = Settings.NovuSubscriberId;
                        break;
                    case "NovuApplicationId":
                        value = Settings.NovuApplicationId;
                        break;
                    case "NovuSubscriberPrefix":
                        value = Settings.NovuSubscriberPrefix;
                        break;
                    case "NovuFirstName":
                        value = Environment.MachineName;
                        break;
                    case "NovuLastName ":
                        value = "Notifier";
                        break;
                    case "NovuEmail":
                        value = "novu@novu.de";
                        break;

                    default:
                        value = string.Empty;
                        break;

                }
                return value;
            } else
            {
                return string.Empty;
            }
        }
        catch (ConfigurationErrorsException ex)
        {
            _logger.Error<AppConfig>($"Configuration error for key '{key}': {ex.Message}");
            throw new InvalidOperationException($"Configuration error for key '{key}': {ex.Message}");
        }
    }
    private AppSettings LoadFromEmbeddedJson()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "BeebopNovu.XPlatProj.appsettings.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new FileNotFoundException("Embedded appsettings.json not found");

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        return JsonSerializer.Deserialize<AppSettings>(json);
    }
}
public class AppSettings
{
    public string DatabasePath { get; set; } = "appdatabase.db";
    public string LogLevel { get; set; } = "Information";
    public bool EnableLogging { get; set; } = true;
    public int MaxLogFiles { get; set; } = 10;
    public string NovuApiKey { get; set; } = string.Empty;
    public string NovuSubscriberId { get; set; } = string.Empty;
    public string NovuApplicationId { get; set; } = string.Empty;
    public string NovuSubscriberPrefix { get; set; } = string.Empty;
    public string LogFilePath { get; set; } = "NotificationService.log";
    public string NovuUrl { get; set; } = "https://api.novu.co";
}