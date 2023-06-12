using System.Globalization;
using Impactt.API.Models;
using Impactt.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Impactt.API.Controllers;

[ApiController]
[Route("api/rooms")]
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

    [HttpGet]
    public async Task<ActionResult<Pagination<RoomModel>>> GetRoomsAsync([FromQuery] GetRoomsQueryModel query)
    {
        var rooms = await _bookingService.GetRoomsAsync(query);

        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomModel>> GetRoomAsync(int id)
    {
        var room = await _bookingService.GetRoomAsync(id);

        if (room == null)
        {
            return NotFound(new { error = "topilmadi" });
        }

        return Ok(room);
    }

    [HttpGet("{id}/availability")]
    public async Task<ActionResult<IEnumerable<AvailableTimeModel>>> GetRoomAvailableTimesAsync(int id, [FromQuery] string date)
    {
        var result = DateOnly.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateOnly);

        if (!result && string.IsNullOrWhiteSpace(date))
        {
            dateOnly = DateOnly.FromDateTime(DateTime.Today);
        }
        else if (!result)
        {
            return BadRequest(new { error = "sana noto'g'ri kiritilgan (dd-MM-yyyy)" });
        }

        if (dateOnly < DateOnly.FromDateTime(DateTime.Today))
        {
            return BadRequest(new { error = "o'tib ketgan sana kiritilgan" });
        }

        var availableTimes = await _bookingService.GetRoomAvailableTimes(id, dateOnly);

        if (availableTimes == null)
        {
            return NotFound(new { error = "topilmadi" });
        }

        return Ok(availableTimes);
    }

    [HttpPost("{id}/book")]
    public async Task<IActionResult> BookRoomAsync(int id, [FromBody] BookedTimeModel model)
    {
        var bookedTime = await _bookingService.BookRoomAsync(id, model);

        if (bookedTime == null)
        {
            return NotFound(new { error = "topilmadi" });
        }

        return new ObjectResult(new { message = "xona muvaffaqiyatli band qilindi" }) { StatusCode = 201 };
    }
}