using System.Text.Json.Serialization;

namespace BeebopNoteApp.Domain.Models;

public partial class Action
{
    [JsonPropertyName("buttons")]
    public object[] Buttons { get; set; } = [];
}