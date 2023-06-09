namespace Impactt.API.Models;

public class GetRoomsQueryModel
{
    private int _page = 1;
    private int _pageSize = 10;

    public int Page {
        get { return _page; }
        set { _page = value > 0 ? value : 1; }
    }

    public int PageSize {
        get { return _pageSize; }
        set { _pageSize = value > 0 ? value : 10; }
    }
    
    public string Search { get; set; }
    public string Type { get; set; }
}