using System.Text.Json.Serialization;

namespace Impactt.API.Models;

public class Resident
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}