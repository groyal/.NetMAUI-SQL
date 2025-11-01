using System.Text.Json.Serialization;

namespace BeebopNoteApp.Domain.Models;

public partial class Payload
{
    [JsonPropertyName("__source")]
    public string Source { get; set; } = string.Empty;
}