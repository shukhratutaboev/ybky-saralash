using System.Text.Json.Serialization;

namespace Impactt.API.Models;

public class ResidentModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}