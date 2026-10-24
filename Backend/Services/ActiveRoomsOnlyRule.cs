public class ActiveRoomsOnlyRule : IReservationRule
{
    private readonly BookingContext _db;

    public ActiveRoomsOnlyRule(BookingContext db)
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
                Code = "R7",
                Message = "Room does not exist"
            };
        }
        if(room.IsActive == false)
        {
            return new Error
            {
                Code = "R7",
                Message = "This room is inactive and cannot be booked right now"
            };
        }
        return null;
    }
}