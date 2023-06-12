using Impactt.API.Models;
using Impactt.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impactt.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IBookingService _bookingService;

    public BookingController(
        ILogger<BookingController> logger,
        IBookingService bookingService)
    {
        _logger = logger;
        _bookingService = bookingService;
    }

    [HttpGet("rooms")]
    public async Task<ActionResult<Pagination<RoomModel>>> GetRoomsAsync([FromQuery] GetRoomsQueryModel query)
    {
        var rooms = await _bookingService.GetRoomsAsync(query);

        return Ok(rooms);
    }

    [HttpGet("rooms/{id}")]
    public async Task<ActionResult<RoomModel>> GetRoomAsync(int id)
    {
        var room = await _bookingService.GetRoomAsync(id);

        if (room == null)
        {
            return NotFound(new { error = "topilmadi" });
        }

        return Ok(room);
    }
}