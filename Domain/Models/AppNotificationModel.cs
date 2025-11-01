using System;

namespace BeebopNoteApp.Domain.Models;

public class AppNotificationModel
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public DateTime ReceivedDate { get; set; }
    public string Subscriberid { get; set; } = string.Empty;
    public bool IsRead { get; set; }

    public string Subject { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
}