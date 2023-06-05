namespace Impactt.API.Entities;

public class BookedTime
{
    public long Id { get; set; }
    public long RoomId { get; set; }
    public Room Room { get; set; }
    public string Resident { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}