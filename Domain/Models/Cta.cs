using System.Text.Json.Serialization;

namespace BeebopNoteApp.Domain.Models;

public partial class Cta
{
    [JsonPropertyName("action")]
    public Action Action { get; set; } = null!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}