using Microsoft.EntityFrameworkCore;
public static class ReservationEndpoints
{
    public static void MapReservationEndpoints (this WebApplication app)
    {
        app.MapGet("/reservations", async (int? matchingRoom, int? matchingOrganizer, ReservationStatus? matchingStatus, DateTime? dateRangeStart, DateTime? dateRangeEnd, BookingContext db)=>
        {
            var query = db.Reservations.AsQueryable();
            if(matchingRoom != null)
            {
                query = query.Where(r => r.RoomId == matchingRoom);
            }
            if(matchingOrganizer != null)
            {
                query = query.Where(r => r.OrganizerId == matchingOrganizer);
            }
            if(matchingStatus != null)
            {
                query = query.Where(r => r.Status == matchingStatus);
            }
            if(dateRangeStart != null && dateRangeEnd != null)
            {
                query = query.Where(r => r.Start >= dateRangeStart && r.End <= dateRangeEnd);
            }
            return await query.ToListAsync();
        });
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
            if(reservation.Status == ReservationStatus.Completed || reservation.End < DateTime.UtcNow)
            {
                return Results.BadRequest(new Error
                {
                    Code = "2",
                    Message = "Meeting has already ended"
                });
            }
            var now = DateTime.UtcNow;
            if(reservation.Start > now)
            {
                return Results.BadRequest(new Error
                {
                    Code="3",
                    Message="Meeting hasn't started yet"
                });
            }
            reservation.Status = ReservationStatus.Completed;
            reservation.End = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return Results.Ok(reservation);
        });
    }
}