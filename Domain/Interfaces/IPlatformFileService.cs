using System.IO;

namespace BeebopNoteApp.Domain.Interfaces;

public interface IPlatformFileService
{
    string GetApplicationDataDirectory(string appName);
    Stream GetAsset(string fileName);
    string GetAssetPath(string fileName);
    string GetDatabasePath();
    string GetFilesDirectory();
    string GetCacheDirectory();
    string GetLogsDirectory();
}