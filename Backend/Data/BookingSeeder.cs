public static class BookingSeeder
{
    public static void Seed(BookingContext db)
    {
        db.Rooms.Add(new Room {Name = "Kambarys 1", Capacity = 3, Floor = "C10", Equipment = { EquipmentItems.TVscreen, EquipmentItems.Projector }, IsActive = true});
        db.Rooms.Add(new Room {Name = "Kambarys 2", Capacity = 2, Floor = "A1", IsActive = true});
        db.Rooms.Add(new Room {Name = "Kambarys 3", Capacity = 5, Floor = "A2", Equipment = {}, IsActive = false});

        db.Reservations.Add(new Reservation {RoomId = 1, OrganizerId = 1, MeetingTitle = "informacija", Start = new DateTime(2026, 10, 10), End = new DateTime(2026, 10, 17), Attendees = 3, Status = ReservationStatus.Booked});
        db.Reservations.Add(new Reservation {RoomId = 2, OrganizerId = 1, MeetingTitle = "informacija apie rezervacija", Start = new DateTime(2026, 11, 15), End = new DateTime(2026, 11, 22), Attendees = 1, Status = ReservationStatus.Cancelled});
        db.Reservations.Add(new Reservation {RoomId = 3, OrganizerId = 1, MeetingTitle = "informacija apie rezervacija 0", Start = new DateTime(2026, 11, 10), End = new DateTime(2026, 11, 15), Attendees = 2, Status = ReservationStatus.Completed});
        //db.Reservations.Add(new Reservation {RoomId = 1, OrganizerId = 2, MeetingTitle = "test", Start = new DateTime(2026, 11, 22), End = new DateTime(2026, 11, 24), Attendees = 50, Status = ReservationStatus.Booked});

        db.Users.Add(new User { FirstName = "Vardenis", LastName = "Pavardenis"});
        db.Users.Add(new User { FirstName = "Vardenis1", LastName = "Pavardenis1"});
        db.Users.Add(new User { FirstName = "Vardenis2", LastName = "Pavardenis2"});
        
        db.SaveChanges();
    }
}