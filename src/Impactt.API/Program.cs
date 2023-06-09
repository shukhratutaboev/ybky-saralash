using Impactt.API.Data;
using Impactt.API.Repositories;
using Impactt.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ImpacttDB>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ImpacttDB"));
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddScoped<IRoomsRepository, RoomsRepository>();
builder.Services.AddScoped<IBookedTimesRepository, BookedTimesRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

var serviceProvider = builder.Services.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ImpacttDB>();
    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("Database migration successful!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error applying database migrations: " + ex.Message);
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
