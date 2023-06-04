using System.Text.Json.Serialization;

namespace Impactt.API.Models;

public class Pagination<T> where T : class
{
    private int _pageSize = 10;
    private int _maxPageSize = 50;

    [JsonPropertyName("page")]
    public int PageNumber { get; set; } = 1;

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > _maxPageSize) ? _maxPageSize : (value < 1) ? 1 : value;
    }

    [JsonPropertyName("results")]
    public IEnumerable<T> Results { get; set; }
}