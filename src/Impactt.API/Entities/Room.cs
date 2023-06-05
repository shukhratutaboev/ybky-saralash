namespace Impactt.API.Entities;

public class Room
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public int Capacity { get; set; }
    public ICollection<BookedTime> BookedTimes { get; set; }
}