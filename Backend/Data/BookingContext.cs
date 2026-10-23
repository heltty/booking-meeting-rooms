using Microsoft.EntityFrameworkCore;

public class BookingContext : DbContext
{
    public BookingContext(DbContextOptions<BookingContext> options)
        : base(options)
    {
    }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set;}
    public DbSet<User> Users {get; set;}
}