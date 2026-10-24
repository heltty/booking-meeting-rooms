using Microsoft.EntityFrameworkCore;
public static class ReservationEndpoints
{
    public static void MapReservationEndpoints (this WebApplication app)
    {
        app.MapGet("/reservations", async(BookingContext db) => await db.Reservations.ToListAsync());
    }
}