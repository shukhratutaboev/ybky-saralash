using System.Text.Json.Serialization;
using Impactt.API.Converters;

namespace Impactt.API.Models;

public class TimePeriodModel
{
    [JsonPropertyName("start")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime Start { get; set; }

    [JsonPropertyName("end")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime End { get; set; }
}