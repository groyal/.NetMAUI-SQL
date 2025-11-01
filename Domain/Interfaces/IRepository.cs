using System.Collections.Generic;
using System.Threading.Tasks;
using BeebopNoteApp.Domain.Models;
using static BeebopNoteApp.Domain.SQLScheme;

namespace BeebopNoteApp.Domain.Interfaces;
public interface IRepository
{
    Task InitializeDatabase();

    // Generic Methods
    Task UpsertRow(ISQLTable table, Dictionary<string, object> values);

    Task<List<Dictionary<string, object>>> ReadRows(
        ISQLTable table,
        string[]? selectColumns = null,
        string? whereClause = null,
        Dictionary<string, object>? whereParams = null);

    Task<int> UpdateRows(
        ISQLTable table,
        Dictionary<string, object> setValues,
        string whereClause,
        Dictionary<string, object> whereParams);

    Task<int> DeleteRows(
        ISQLTable table,
        string whereClause,
        Dictionary<string, object> whereParams);

    // Specific Methods
    Task<bool> CheckForTableChange();
    Task<string?> GetSubscriberId();
    Task SetSubscriberId(string subscriberId);
    Task<List<AppNotificationModel>> GetAllNotifications();
    Task SaveNotification(AppNotificationModel notification);
    Task MarkAsRead(string notificationId);
    Task<SettingsModel> GetSettings();
    Task UpdateSettings(SettingsModel settings);
    Task<AppNotificationModel?> GetNotificationById(string notificationId);
}