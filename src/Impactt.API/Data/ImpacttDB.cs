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
        modelBuilder.Entity<BookedTime>().ToTable("booked_time");
        modelBuilder.Entity<Room>().ToTable("room");

        modelBuilder.Entity<BookedTime>(b =>
        {
            b.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityByDefaultColumn();
            
            b.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Room>(r =>
        {
            r.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityByDefaultColumn();
            
            r.HasKey(e => e.Id);

            r.HasMany(e => e.BookedTimes)
                .WithOne(e => e.Room)
                .HasForeignKey(e => e.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}