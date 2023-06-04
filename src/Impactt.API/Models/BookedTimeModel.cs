using System.Text.Json.Serialization;

namespace Impactt.API.Models;

public class BookedTimeModel : TimePeriodModel
{
    [JsonPropertyName("resident")]
    public ResidentModel Resident { get; set; }
}