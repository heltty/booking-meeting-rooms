using Microsoft.Extensions.Options;

public class BookingHorizonRule : IReservationRule
{
    private readonly BookingLimits _opt;
    public BookingHorizonRule(IOptions<BookingLimits> opt)
    {
        _opt = opt.Value;
    }
    public Error? Validate(CreateReservationRequest req)
    {
        var days = (req.Start - DateTime.UtcNow).TotalDays;
        if(days > _opt.BookingHorizonRule)
        {
            return new Error
            {
                Code = "R6",
                Message = $"A reservation cannot start more than {_opt.BookingHorizonRule} days in the future"
            };
        }
        return null;
    }
}