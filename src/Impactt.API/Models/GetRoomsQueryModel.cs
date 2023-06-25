using Microsoft.AspNetCore.Mvc;

namespace Impactt.API.Models;

public class GetRoomsQueryModel
{
    private int _page = 1;
    private int _pageSize = 10;

    [FromQuery(Name = "page")]
    public int Page {
        get { return _page; }
        set { _page = value > 0 ? value : 1; }
    }

    [FromQuery(Name = "page_size")]
    public int PageSize {
        get { return _pageSize; }
        set { _pageSize = value > 0 ? value : 10; }
    }
    
    [FromQuery(Name = "search")]
    public string Search { get; set; }
    
    [FromQuery(Name = "type")]
    public string Type { get; set; }
}
