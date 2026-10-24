using Microsoft.Extensions.Options;

public class MaximumDurationRule : IReservationRule
{
    private readonly BookingLimits _opt;

    public MaximumDurationRule(IOptions<BookingLimits> opt)
    {
        _opt = opt.Value;
    }

    public Error? Validate(CreateReservationRequest req)
    {
        var days = (req.End - req.Start).TotalDays;
        if(days > _opt.MaxDurationRule)
        {
            return new Error
            {
                Code = "R1",
                Message = $"Reservation duration cannot last longer than {_opt.MaxDurationRule} days"
            };
        }
        return null;
    }
}