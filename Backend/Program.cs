using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookingContext>(options => options.UseInMemoryDatabase("BookingDb"));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookingContext>();

    db.Rooms.Add(new Room {Name = "Kambarys 1", Capacity = 3, Floor = "C10", Equipment = { EquipmentItems.TVscreen, EquipmentItems.Projector }, IsActive = true});
    db.Rooms.Add(new Room {Name = "Kambarys 2", Capacity = 2, Floor = "A1", IsActive = true});
    db.Rooms.Add(new Room {Name = "Kambarys 3", Capacity = 5, Floor = "A2", Equipment = {}, IsActive = false});

    db.Reservations.Add(new Reservation {RoomId = 1, OrganizerId = 1, MeetingTitle = "informacija", Start = new DateTime(2026, 10, 10), End = new DateTime(2026, 10, 17), Attendees = 3, Status = ReservationStatus.Booked});
    db.Reservations.Add(new Reservation {RoomId = 2, OrganizerId = 1, MeetingTitle = "informacija apie rezervacija", Start = new DateTime(2026, 11, 15), End = new DateTime(2026, 11, 22), Attendees = 1, Status = ReservationStatus.Cancelled});
    db.Reservations.Add(new Reservation {RoomId = 3, OrganizerId = 1, MeetingTitle = "informacija apie rezervacija 0", Start = new DateTime(2026, 11, 10), End = new DateTime(2026, 11, 15), Attendees = 2, Status = ReservationStatus.Completed});
    
    db.Users.Add(new User { FirstName = "Vardenis", LastName = "Pavardenis"});
    db.Users.Add(new User { FirstName = "Vardenis1", LastName = "Pavardenis1"});
    db.Users.Add(new User { FirstName = "Vardenis2", LastName = "Pavardenis2"});

    db.SaveChanges();
}

app.MapGet("/rooms", async (int? minCapacity, string? matchingFloor, EquipmentItems? matchingEquipment, BookingContext db) =>
{
    var query = db.Rooms.AsQueryable();
    if (minCapacity != null)
    {
        query = query.Where(r => r.Capacity >= minCapacity);
    }
    if(matchingEquipment != null)
    {
        query = query.Where(r => r.Equipment.Contains(matchingEquipment.Value));
    }
    if (matchingFloor != null)
    {
        query = query.Where(r => r.Floor == matchingFloor);
    }
    return await query.ToListAsync();
});
app.MapGet("/reservations", async(BookingContext db) => await db.Reservations.ToListAsync());
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

app.Run();
