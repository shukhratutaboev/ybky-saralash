using System.Text.Json;
using Impactt.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Impactt.API.Data;

public class ImpacttDB : DbContext
{
    public ImpacttDB(DbContextOptions<ImpacttDB> options) : base(options) { }

    public DbSet<BookedTime> BookedTimes { get; set; }
    public DbSet<Room> Rooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookedTime>().ToTable("booked_times");
        modelBuilder.Entity<Room>().ToTable("rooms");

        var json = File.ReadAllText("Data/SeedData.json");
        var rooms = JsonSerializer.Deserialize<List<Room>>(json);

        modelBuilder.Entity<Room>().HasData(rooms);

        modelBuilder.Entity<BookedTime>(b =>
        {
            b.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityByDefaultColumn();

            b.Property(e => e.Resident)
                .IsRequired()
                .HasMaxLength(50);

            b.HasIndex(e => e.StartTime);
            b.HasIndex(e => e.EndTime);
            
            b.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Room>(r =>
        {
            r.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityByDefaultColumn();

            r.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);

            r.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);
            
            r.HasKey(e => e.Id);

            r.HasMany(e => e.BookedTimes)
                .WithOne(e => e.Room)
                .HasForeignKey(e => e.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}