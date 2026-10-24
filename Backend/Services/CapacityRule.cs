public class CapacityRule : IReservationRule
{
    private readonly BookingContext _db;

    public CapacityRule(BookingContext db)
    {
        _db = db;
    }
    public Error? Validate(CreateReservationRequest req)
    {
        var room = _db.Rooms.Find(req.RoomId);
        if(room == null)
        {
            return new Error
            {
                Code = "R5",
                Message = "Selected room does not exist"
            };
        }
        if(req.Attendees > room.Capacity)
        {
            return new Error
            {
                Code = "R5",
                Message = "Number of attendees cannot exceed the room capacity"
            };
        }
        return null;
    }
}