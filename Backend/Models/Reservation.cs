public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int OrganizerId { get; set; }
    public required string MeetingTitle { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int Attendees { get; set; }
    public ReservationStatus Status { get; set; }
}