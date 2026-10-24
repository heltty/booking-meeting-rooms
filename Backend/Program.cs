using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookingContext>(options => options.UseInMemoryDatabase("BookingDb"));

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<BookingLimits>(
    builder.Configuration.GetSection("BookingRules")
);

builder.Services.AddScoped<IReservationRule, MaximumDurationRule>();
builder.Services.AddScoped<IReservationRule, NoOverlappingRoomRule>();
builder.Services.AddScoped<IReservationRule, NoOverlappingOrganizerRule>();
builder.Services.AddScoped<IReservationRule, ValidTimeRangeRule>();
builder.Services.AddScoped<IReservationRule, CapacityRule>();
builder.Services.AddScoped<IReservationRule, BookingHorizonRule>();
builder.Services.AddScoped<IReservationRule, ActiveRoomsOnlyRule>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookingContext>();

    BookingSeeder.Seed(db);
}
app.MapRoomEndpoints();
app.MapUserEndpoints();
app.MapReservationEndpoints();

app.Run();
