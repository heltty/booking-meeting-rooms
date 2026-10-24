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

    BookingSeeder.Seed(db);
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
