public class NoOverlappingOrganizerRule : IReservationRule
{
    private readonly BookingContext _db;

    public NoOverlappingOrganizerRule(BookingContext db)
    {
        _db = db;
    }
    public Error? Validate(CreateReservationRequest req)
    {
        bool overlaps = _db.Reservations.Any(
            r => req.OrganizerId == r.OrganizerId &&
            r.Status == ReservationStatus.Booked &&
            req.End > r.Start &&
            req.Start < r.End);
        if(overlaps == true)
        {
            return new Error
            {
                Code = "R3",
                Message = "The same user cannot organize two meetings that overlap in date, even in different rooms"
            };
        }
        return null;
    }
}