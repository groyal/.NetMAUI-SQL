using System.Text.Json.Serialization;

namespace BeebopNoteApp.Domain.Models;

public partial class Template
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("tags")]
    public object[] Tags { get; set; } = [];

    [JsonPropertyName("id")]
    public string TemplateId { get; set; } = string.Empty;
}