using System.Text.Json.Serialization;

namespace Impactt.API.Models;

public class BookedTime : TimePeriod
{
    [JsonPropertyName("resident")]
    public Resident Resident { get; set; }
}