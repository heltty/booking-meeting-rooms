public class NoOverlappingRoomRule : IReservationRule
{
    private readonly BookingContext _db;

    public NoOverlappingRoomRule(BookingContext db)
    {
        _db = db;
    }

    public Error? Validate(CreateReservationRequest req)
    {
        bool overlaps = _db.Reservations.Any(r => req.End > r.Start && req.Start < r.End && r.RoomId == req.RoomId && r.Status == ReservationStatus.Booked);
        if(overlaps == true)
        {
            return new Error
            {
                Code = "R2",
                Message = "A room cannot have two active reservations"
            };
        }
        return null;
    }
}