public class ValidTimeRangeRule : IReservationRule
{
    public Error? Validate(CreateReservationRequest req)
    {
        if(req.Start < DateTime.UtcNow)
        {
            return new Error
            {
                Code = "R4",
                Message = "Start must happen in the future"
            };
        }
        return null;
    }
}