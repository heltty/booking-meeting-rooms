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
        app.MapPatch("/reservations/{id}/cancel", async (int id, BookingContext db) =>
        {
            var reservation = await db.Reservations.FindAsync(id);
            if(reservation == null)
            {
                return Results.NotFound();
            }
            if(reservation.Status == ReservationStatus.Completed)
            {
                return Results.BadRequest(new Error
                {
                    Code = "1",
                    Message = "You cannot cancel completed meeting"
                });
            }
            if(reservation.Status == ReservationStatus.Cancelled)
            {
                return Results.BadRequest(new Error
                {
                    Code = "2",
                    Message = "The meeting has already been cancelled"
                });
            }
            reservation.Status = ReservationStatus.Cancelled;
            await db.SaveChangesAsync();
            return Results.Ok(reservation);
        });
        app.MapPatch("/reservations/{id}/complete", async (int id, BookingContext db)=>
        {
            var reservation = await db.Reservations.FindAsync(id);
            if(reservation == null)
            {
                return Results.NotFound();
            }
            if(reservation.Status == ReservationStatus.Cancelled)
            {
                return Results.BadRequest(new Error
                {
                    Code = "1",
                    Message = "You can't end a cancelled meeting"
                });
            }
            if(reservation.Status == ReservationStatus.Completed)
            {
                return Results.BadRequest(new Error
                {
                    Code = "2",
                    Message = "Meeting has already ended"
                });
            }
            reservation.Status = ReservationStatus.Completed;
            reservation.End = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return Results.Ok(reservation);
        });
    }
}