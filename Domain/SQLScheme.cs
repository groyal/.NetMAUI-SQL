namespace BeebopNoteApp.Domain;

public static class SQLScheme
{
    public interface ISQLTable
    {
        string Name { get; }
        SQLColumn[] GetColumns();
    }

    public record struct SQLColumn(
        string Name,
        string Type,
        string Constraints
    );

    public static class SQLite
    {
        public static readonly ISQLTable[] AllTables =
        [
            new Tables.Notifications(),
                new Tables.Settings(),
                new Tables.TableChange()
        ];

        public static class Tables
        {
            public class Notifications : ISQLTable
            {
                public string Name => "Notifications";

                public static class Columns
                {
                    public static readonly SQLColumn Id = new("Id", "STRING", "PRIMARY KEY");
                    public static readonly SQLColumn Title = new("Title", "TEXT", "");
                    public static readonly SQLColumn Message = new("Message", "TEXT", "");
                    public static readonly SQLColumn ReceivedDate = new("ReceivedDate", "DATETIME", "");
                    public static readonly SQLColumn IsRead = new("IsRead", "BOOLEAN", "");
                    public static readonly SQLColumn SubscriberId = new("subscriberid", "TEXT", "");
                    public static readonly SQLColumn Subject = new("Subject", "TEXT", "");
                    public static readonly SQLColumn Priority = new("priority", "TEXT", "");
                    public static readonly SQLColumn IsToast = new("IsToast", "BOOLEAN", "");
                }

                public SQLColumn[] GetColumns()
                {
                    return [
                    Columns.Id,
                        Columns.Title,
                        Columns.Message,
                        Columns.ReceivedDate,
                        Columns.IsRead,
                        Columns.SubscriberId,
                        Columns.Subject,
                        Columns.Priority,
                        Columns.IsToast
                ];
                }
            }

            public class Settings : ISQLTable
            {
                public string Name => "Settings";

                public static class Columns
                {
                    public static readonly SQLColumn Id = new("Id", "INTEGER", "PRIMARY KEY AUTOINCREMENT");
                    public static readonly SQLColumn SubscriberId = new("SubscriberId", "TEXT", "");
                    public static readonly SQLColumn FirstName = new("FIRSTNAME", "TEXT", "");
                    public static readonly SQLColumn SecondName = new("SECONDNAME", "TEXT", "");
                    public static readonly SQLColumn Email = new("Email", "TEXT", "");
                }

                public SQLColumn[] GetColumns()
                {
                    return [
                    Columns.Id,
                        Columns.SubscriberId,
                        Columns.FirstName,
                        Columns.SecondName,
                        Columns.Email
                ];
                }
            }

            public class TableChange : ISQLTable
            {
                public string Name => "Tablechange";

                public static class Columns
                {
                    public static readonly SQLColumn Id = new("Id", "INTEGER", "PRIMARY KEY AUTOINCREMENT");
                    public static readonly SQLColumn HasChanged = new("has_changed", "INTEGER", "");
                }

                public SQLColumn[] GetColumns()
                {
                    return [
                    Columns.Id,
                        Columns.HasChanged
                ];
                }
            }
        }
    }
}