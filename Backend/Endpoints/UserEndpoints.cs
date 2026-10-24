using Microsoft.EntityFrameworkCore;

public static class UserEndpoints
{
    public static void MapUserEndpoints (this WebApplication app)
    {
        app.MapGet("/users", async(BookingContext db) => 
            await db.Users
            .Select(u => new UserSummary
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Id = u.Id,
                ActiveReservations = db.Reservations.Count(b => b.OrganizerId == u.Id && b.Status == ReservationStatus.Booked)
            })
            .ToListAsync());
        
        app.MapGet("/users/{id}/active-reservations", async (int id, BookingContext db) =>
            await db.Reservations.CountAsync(b => b.OrganizerId == id && b.Status == ReservationStatus.Booked));
    }
}