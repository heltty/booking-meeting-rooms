using Microsoft.EntityFrameworkCore;
public static class ReservationEndpoints
{
    public static void MapReservationEndpoints (this WebApplication app)
    {
        app.MapGet("/reservations", async(BookingContext db) => await db.Reservations.ToListAsync());
        app.MapPost("/reservations", async(CreateReservationRequest req, BookingContext db, IEnumerable<IReservationRule> rules) =>
        {
            foreach(var rule in rules)
            {
                var error = rule.Validate(req);
                if(error != null)
                {
                    return Results.BadRequest(error);
                }
            }
            var reservation = new Reservation
            {
                RoomId = req.RoomId,
                OrganizerId = req.OrganizerId,
                MeetingTitle = req.MeetingTitle,
                Start = req.Start,
                End = req.End,
                Attendees = req.Attendees,
                Status = ReservationStatus.Booked
            };
            db.Reservations.Add(reservation);
            await db.SaveChangesAsync();

            return Results.Created($"/reservations/{reservation.Id}", reservation);
        });
    }
}