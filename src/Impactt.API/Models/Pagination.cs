using System.Text.Json.Serialization;

namespace Impactt.API.Models;

public class Pagination<T> where T : class
{
    [JsonPropertyName("page")]
    public int PageNumber { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("results")]
    public IEnumerable<T> Results { get; set; }
}