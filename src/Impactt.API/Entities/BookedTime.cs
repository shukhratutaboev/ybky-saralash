using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Impactt.API.Entities;

public class BookedTime
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long RoomId { get; set; }

    [ForeignKey(nameof(RoomId))]
    public Room Room { get; set; }

    public string Resident { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }
}