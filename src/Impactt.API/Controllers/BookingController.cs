using System.Globalization;
using Impactt.API.Exceptions;
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
            throw new ApiException("sana noto'g'ri kiritilgan (dd-MM-yyyy)", 400);
        }

        var availableTimes = await _bookingService.GetRoomAvailableTimes(id, dateOnly);

        return Ok(availableTimes);
    }

    [HttpPost("{id}/book")]
    public async Task<IActionResult> BookRoomAsync(int id, [FromBody] BookedTimeModel model)
    {
        await _bookingService.BookRoomAsync(id, model);

        return new ObjectResult(new { message = "xona muvaffaqiyatli band qilindi" }) { StatusCode = 201 };
    }
}