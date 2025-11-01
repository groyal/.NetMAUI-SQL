using log4net;
using log4net.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using BeebopNoteApp.Domain.Interfaces;

namespace BeebopNoteApp.Infrastructure.Services;

public class LogConfigurator
{
    private readonly IPlatformFileService _fileService;

    public LogConfigurator(IPlatformFileService fileService)
    {
        _fileService = fileService;
    }

    public static readonly string ApplicationName = "BeebopNovu";
    public static readonly string LogFileName = "NotificationService.log";

    public void ConfigureLog4Net()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== Configuring log4net from embedded resource ===");

            // 1. Get log directory (platform-specific)
            string artifactsDirectory = _fileService.GetApplicationDataDirectory(ApplicationName);
            Directory.CreateDirectory(artifactsDirectory);
            string logFilePath = Path.Combine(artifactsDirectory, LogFileName);

            System.Diagnostics.Debug.WriteLine($"Log file path: {logFilePath}");

            // 2. Set log4net global property for dynamic file path
            log4net.GlobalContext.Properties["LogFilePath"] = logFilePath;

            // 3. Load log4net config from EMBEDDED RESOURCE
            var assembly = Assembly.GetExecutingAssembly();
            // Full name: {Namespace}.{Folder}.{Filename}
            string resourceName = "BeebopNovu.XPlatProj.Resources.log4net.config";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                var names = assembly.GetManifestResourceNames();
                System.Diagnostics.Debug.WriteLine("Available embedded resources:");
                foreach (var name in names)
                    System.Diagnostics.Debug.WriteLine($"  - {name}");

                throw new FileNotFoundException($"Embedded resource not found: {resourceName}");
            }

            XmlConfigurator.Configure(stream);
            System.Diagnostics.Debug.WriteLine("✅ log4net configured successfully from embedded resource");

            // After XmlConfigurator.Configure(stream);
            var log = LogManager.GetLogger(typeof(LogConfigurator));
            log.Info("✅ Logging system initialized");

            // Force a log entry that will definitely write to file
            log.Info("📱 Android log file test entry");

            // Also print path to Logcat
            System.Diagnostics.Debug.WriteLine($"📱 Expected log file: {logFilePath}");

            // 4. Test logging
            log.Info("Logging system initialized successfully");
            log.Info($"Log file: {logFilePath}");

            System.Diagnostics.Debug.WriteLine("Log test message sent");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"❌ ERROR configuring log4net: {ex}");
            BasicConfigurator.Configure(); // fallback to console-only
        }
    }
}