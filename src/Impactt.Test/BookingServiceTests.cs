using Impactt.API.Entities;
using Impactt.API.Exceptions;
using Impactt.API.Models;
using Impactt.API.Repositories;
using Impactt.API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Impactt.Test;

public class BookingServiceTests
{
    private readonly BookingService _bookingService;
    private readonly Mock<ILogger<BookingService>> _loggerMock;
    private readonly Mock<IRoomsRepository> _roomsRepositoryMock;
    private readonly Mock<IBookedTimesRepository> _bookedTimesRepositoryMock;

    public BookingServiceTests()
    {
        _loggerMock = new Mock<ILogger<BookingService>>();
        _roomsRepositoryMock = new Mock<IRoomsRepository>();
        _bookedTimesRepositoryMock = new Mock<IBookedTimesRepository>();

        _roomsRepositoryMock
            .Setup(e => e.GetRoomsQueryableAsync())
            .ReturnsAsync(GetTestRooms().AsQueryable());

        _roomsRepositoryMock
            .Setup(e => e.GetRoomAsync(It.IsAny<long>()))
            .ReturnsAsync((long id) => GetTestRooms().FirstOrDefault(e => e.Id == id));

        _bookedTimesRepositoryMock
            .Setup(e => e.GetRoomBookedTimesAsync(It.IsAny<long>(), It.IsAny<DateOnly>()))
            .ReturnsAsync((long roomId, DateOnly date) => GetTestBookedTimes(roomId, date).AsEnumerable());

        _bookingService = new BookingService(
            _loggerMock.Object,
            _roomsRepositoryMock.Object,
            _bookedTimesRepositoryMock.Object
        );
    }

    [Fact]
    public async Task GetRoomsAsync_WithValidQuery_ReturnsPaginationOfRoomModels()
    {
        var query = new GetRoomsQueryModel
        {
            Page = 1,
            PageSize = 3,
            Type = "team"
        };

        var result = await _bookingService.GetRoomsAsync(query);

        Assert.NotNull(result);
        Assert.Equal(3, result.Results.Count());
        Assert.Equal(4, result.Count);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(3, result.PageSize);
    }

    [Fact]
    public async Task GetRoomsAsync_WithInvalidQuery_ReturnsEmptyPaginationOfRoomModels()
    {
        var query = new GetRoomsQueryModel
        {
            Type = "focus",
            Search = "any room"
        };

        var result = await _bookingService.GetRoomsAsync(query);

        Assert.NotNull(result);
        Assert.Empty(result.Results);
        Assert.Equal(0, result.Count);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task GetRoomAsync_WithValidId_ReturnsRoomModel()
    {
        var result = await _bookingService.GetRoomAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("mytaxi", result.Name);
        Assert.Equal("focus", result.Type);
        Assert.Equal(1, result.Capacity);
    }

    [Fact]
    public async Task GetRoomAsync_WithInvalidId_ThrowsApiException()
    {
        var act = async () => await _bookingService.GetRoomAsync(20);

        var exception = await Assert.ThrowsAsync<ApiException>(act);

        Assert.Equal("topilmadi", exception.Message);
        Assert.Equal(404, exception.StatusCode);
    }

    [Fact]
    public async Task GetRoomAvailableTimesAsync_WithValidId_ReturnsListOfAvailableTimeModels()
    {
        var result = await _bookingService.GetRoomAvailableTimesAsync(1, DateOnly.FromDateTime(DateTime.Now.AddDays(1)));

        Assert.NotNull(result);
        Assert.Equal(6, result.Count());
    }

    [Fact]
    public async Task GetRoomAvailableTimesAsync_WithInvalidId_ThrowsApiException()
    {
        var act = async () => await _bookingService.GetRoomAvailableTimesAsync(100, DateOnly.FromDateTime(DateTime.Now.AddDays(1)));

        var exception = await Assert.ThrowsAsync<ApiException>(act);

        Assert.Equal("topilmadi", exception.Message);
        Assert.Equal(404, exception.StatusCode);
    }

    [Fact]
    public async Task GetRoomAvailableTimesAsync_WithInvalidDate_ThrowsApiException()
    {
        var act = async () => await _bookingService.GetRoomAvailableTimesAsync(1, DateOnly.FromDateTime(DateTime.Now.AddDays(-1)));

        var exception = await Assert.ThrowsAsync<ApiException>(act);

        Assert.Equal("o'tib ketgan sana kiritilgan", exception.Message);
        Assert.Equal(400, exception.StatusCode);
    }

    [Fact]
    public async Task GetRoomAvailableTimesAsync_WithEmptyBookedTimes_ReturnsOneAvailableTimeModel()
    {
        var result = await _bookingService.GetRoomAvailableTimesAsync(1, DateOnly.FromDateTime(DateTime.Now));

        var now = DateTime.Now.ToLocalTime();
        var end = new DateTime(now.Year, now.Month, now.Day + 1);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.True(result.First().Start - now < TimeSpan.FromMinutes(1));
        Assert.Equal(result.First().End, end);
    }

    [Fact]
    public async Task BookRoomAsync_WithValidModel_ReturnsModelAndAvailabeliTimesIncreaseByOne()
    {
        var bookedTimes = new List<BookedTime>();

        _bookedTimesRepositoryMock
            .Setup(e => e.AddBookedTimeAsync(It.IsAny<BookedTime>()))
            .Callback<BookedTime>(e => bookedTimes.Add(e));

        _bookedTimesRepositoryMock
            .Setup(e => e.IsAvailableAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        _bookedTimesRepositoryMock
            .Setup(e => e.GetRoomBookedTimesAsync(It.IsAny<long>(), It.IsAny<DateOnly>()))
            .ReturnsAsync((long roomId, DateOnly date) => bookedTimes.AsEnumerable());

        var model = new BookedTimeModel
        {
            Start = DateTime.Now.ToLocalTime().AddMinutes(10),
            End = DateTime.Now.ToLocalTime().AddMinutes(40),
            Resident = new ResidentModel
            {
                Name = "John Doe"
            }
        };

        var before = await _bookingService.GetRoomAvailableTimesAsync(1, DateOnly.FromDateTime(DateTime.Now));

        var result = await _bookingService.BookRoomAsync(1, model);

        var after = await _bookingService.GetRoomAvailableTimesAsync(1, DateOnly.FromDateTime(DateTime.Now));

        Assert.NotNull(result);
        Assert.Single(before);
        Assert.Equal(2, after.Count());
    }

    [Fact]
    public async Task BookRoomAsync_WithInValidTime_ThrowsApiException()
    {
        var model = new BookedTimeModel
        {
            Start = DateTime.Now.ToLocalTime().AddMinutes(-10),
            End = DateTime.Now.ToLocalTime().AddMinutes(40),
            Resident = new ResidentModel
            {
                Name = "John Doe"
            }
        };

        var act = async () => await _bookingService.BookRoomAsync(1, model);

        var exception = await Assert.ThrowsAsync<ApiException>(act);

        Assert.Equal("o'tib ketgan vaqtda buyurtma berish mumkin emas", exception.Message);
        Assert.Equal(400, exception.StatusCode);
    }

    [Fact]
    public async Task BookRoomAsync_WithInValidRoomId_ThrowsApiException()
    {
        var model = new BookedTimeModel
        {
            Start = DateTime.Now.ToLocalTime().AddMinutes(10),
            End = DateTime.Now.ToLocalTime().AddMinutes(40),
            Resident = new ResidentModel
            {
                Name = "John Doe"
            }
        };

        var act = async () => await _bookingService.BookRoomAsync(100, model);

        var exception = await Assert.ThrowsAsync<ApiException>(act);

        Assert.Equal("topilmadi", exception.Message);
        Assert.Equal(404, exception.StatusCode);
    }

    [Fact]
    public async Task BookRoomAsync_WithNotAvailableTime_ThrowsApiException()
    {
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        var bookedTimes = GetTestBookedTimes(1, date);

        _bookedTimesRepositoryMock
            .Setup(e => e.IsAvailableAsync(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync((long roomId, DateTime startTime, DateTime endTime) => !bookedTimes.Where(e => 
                    (e.StartTime >= startTime && e.StartTime < endTime) ||
                        (e.EndTime > startTime && e.EndTime <= endTime) ||
                        (e.StartTime <= startTime && e.EndTime >= endTime))
                .Any());

        var model = new BookedTimeModel
        {
            Start = date.ToDateTime(new TimeOnly(09, 30)),
            End = date.ToDateTime(new TimeOnly(11, 30)),
            Resident = new ResidentModel
            {
                Name = "John Doe"
            }
        };

        var act = async () => await _bookingService.BookRoomAsync(1, model);

        var exception = await Assert.ThrowsAsync<ApiException>(act);

        Assert.Equal("uzr, siz tanlagan vaqtda xona band", exception.Message);
        Assert.Equal(410, exception.StatusCode);
    }

    private List<Room> GetTestRooms() => new()
    {
        new Room { Id = 1, Name = "mytaxi", Type = "focus", Capacity = 1 },
        new Room { Id = 2, Name = "workly", Type = "team", Capacity = 5 },
        new Room { Id = 3, Name = "express24", Type = "conference", Capacity = 15 },
        new Room { Id = 4, Name = "amazon", Type = "team", Capacity = 4 },
        new Room { Id = 5, Name = "google", Type = "team", Capacity = 10 },
        new Room { Id = 6, Name = "meta", Type = "conference", Capacity = 24 },
        new Room { Id = 7, Name = "uber", Type = "focus", Capacity = 2 },
        new Room { Id = 8, Name = "twitter", Type = "conference", Capacity = 20 },
        new Room { Id = 9, Name = "apple", Type = "focus", Capacity = 3 },
        new Room { Id = 10, Name = "microsoft", Type = "team", Capacity = 6 }
    };

    private List<BookedTime> GetTestBookedTimes(long roomId, DateOnly date)
    {
        if (date == DateOnly.FromDateTime(DateTime.Now)) return new();

        return new()
        {
            new() { Id = 1, RoomId = roomId, Resident = "John Doe", StartTime = date.AddDays(-1).ToDateTime(new TimeOnly(23, 10, 00)), EndTime = date.ToDateTime(new TimeOnly(00, 01, 30)) },
            new() { Id = 2, RoomId = roomId, Resident = "John Doe", StartTime = date.ToDateTime(new TimeOnly(10, 00, 00)), EndTime = date.ToDateTime(new TimeOnly(11, 00, 00)) },
            new() { Id = 3, RoomId = roomId, Resident = "John Doe", StartTime = date.ToDateTime(new TimeOnly(12, 00, 00)), EndTime = date.ToDateTime(new TimeOnly(13, 00, 00)) },
            new() { Id = 4, RoomId = roomId, Resident = "John Doe", StartTime = date.ToDateTime(new TimeOnly(14, 30, 00)), EndTime = date.ToDateTime(new TimeOnly(15, 00, 00)) },
            new() { Id = 5, RoomId = roomId, Resident = "John Doe", StartTime = date.ToDateTime(new TimeOnly(16, 00, 00)), EndTime = date.ToDateTime(new TimeOnly(17, 30, 00)) },
            new() { Id = 6, RoomId = roomId, Resident = "John Doe", StartTime = date.ToDateTime(new TimeOnly(18, 00, 00)), EndTime = date.ToDateTime(new TimeOnly(19, 00, 00)) }
        };
    }
}
