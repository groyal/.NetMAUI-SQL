using System.Text.Json.Serialization;

namespace BeebopNoteApp.Domain.Models;

public partial class Data
{
    [JsonPropertyName("PRIORITY")]
    public string Priority { get; set; } = string.Empty;
}