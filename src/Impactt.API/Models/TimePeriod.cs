using System.Text.Json.Serialization;

namespace Impactt.API.Models;

public class TimePeriod
{
    [JsonPropertyName("start")]
    public DateTime Start { get; set; }

    [JsonPropertyName("end")]
    public DateTime End { get; set; }
}