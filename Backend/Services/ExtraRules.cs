public class ExtraRules : IReservationRule
{
    private readonly BookingContext _db;

    public ExtraRules(BookingContext db)
    {
        _db =db;
    }
    public Error? Validate(CreateReservationRequest req)
    {
        bool exists = _db.Users.Any(u => u.Id == req.OrganizerId);
        if (!exists)
        {
            return new Error
            {
                Code="organizerNotFound",
                Message="Selected organizer does not exist"
            };
        }
        return null;
    }
}