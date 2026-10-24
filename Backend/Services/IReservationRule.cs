public interface IReservationRule
{
    Error? Validate(CreateReservationRequest req);
}