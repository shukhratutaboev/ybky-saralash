using System.Text.Json.Serialization;

namespace Impactt.API.Models;

public class RoomModel
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("capacity")]
    public int Capacity { get; set; }
}