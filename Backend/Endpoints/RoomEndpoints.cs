using Microsoft.EntityFrameworkCore;

public static class RoomEndpoints
{
    public static void MapRoomEndpoints(this WebApplication app)
    {
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
    }
}