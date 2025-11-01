using System.Text.Json.Serialization;

namespace BeebopNoteApp.Domain.Models;

public partial class NovuNotification
{
    [JsonPropertyName("data")]
    public Datum[] Data { get; set; } = [];

    [JsonPropertyName("totalCount")]
    public long TotalCount { get; set; }

    [JsonPropertyName("hasMore")]
    public bool HasMore { get; set; }

    [JsonPropertyName("pageSize")]
    public long PageSize { get; set; }

    [JsonPropertyName("page")]
    public long Page { get; set; }
}