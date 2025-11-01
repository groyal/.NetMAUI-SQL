using System.Text.Json.Serialization;

namespace BeebopNoteApp.Domain.Models;

public partial class Subscriber
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("subscriberId")]
    public string SubscriberSubscriberId { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string SubscriberId { get; set; } = string.Empty;
}