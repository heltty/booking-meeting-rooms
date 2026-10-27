# booking-meeting-rooms

## About the project
- The project is a web application about booking meeting rooms. Users are able to see available rooms, they can book those rooms for meetings, and manage the status of said reservations. Backend validates new reservations against the booking R1-R7 rules to prevent invalid bookings.

## Technology used
Backend:
- C# - coding language
- ASP.NET Core (minimal API)
- Entity Framework Core (InMemory) - database management

Frontend:
- React - UI
- Typescript - coding language
- Bootstrap - styling
- React Router - routing between pages
- Vite - building tool

Other:
- VS Code
- Github

## How to run
Backend:
- from booking-meeting-rooms
- cd Backend
- dotnet run

Frontend (in a seperate terminal):
- from booking-meeting-rooms
- cd Frontend
- npm run dev

## Architecture decisions

1. Rooms page (Backend)
- Chose enum for equipment variables, since it's a fixed variety of items (Projector, TV screen, ...), this way in case there's a need to add or update said item instead of writing the item with a chance of misspelling it, enum simply holds int values starting from 0. Also, a room can have several of those items, so I will store Equipment in a List.
2. Users page (Backend)
- "How many active reservations, user has" - User.cs model class doesn't store that value, because it would need constant updating, back-end counts the number from reservations(only Booked) and sends it to front-end through DTO (UserSummary).
3. Reservations page (Backend)
- Chose enum for reservation status variable, since there's 3 possible choices.
- Chose int for Room and Organizer variables, because in case there's a need to rename a room it would cause issues if the variable was a string, by making RoomId and OrganizerId int, I have the ability to track which room is which by a number and simply change it in Room.cs (1.Users page)
***
- Added database seeders, since this database is in-memory, meaning, that once the program is shut down every database element disappears. Having a seeder lets me test the program anytime.
***
- There are two status variables in different models (one being in Room.cs and the other in Reservation.cs), the thing that differs them is:
  - Room.IsActive is for a room, either true or false, this simply means if the room is even up for reservation (in case of a renovation would make it false)
  - Reservation.Status is the reservation status (Booked, Cancelled, Completed). Rule 2 says that active reservations are those that are still happening, that means Booked.
***
- I chose Minimal API instead of using Controllers to handle requests from front-end, because it needs less code and fewer files. To keep Program.cs clean there's a separate folder /Endpoints with one file per topic (rooms, users, reservations).
***
- Each rule (R1-R7) is a separate class implementing interface IReservationRule, and the POST endpoint goes through all of them with a single loop over IEnumerable&lt;ReservationRule&gt;.I chose this over one big service class, because adding a new rule means adding a new class witout touching the existing ones, each rule can be tested on its own, and each rule only receives the dependencies (BookingLimits or BookingContext) it needs (for example limits from appsettings.json and database).
***
- For Rule 2 and Rule 3 the assignment didn't specify how Completed status reservations affect these rules, so only Booked status reservations are treated as blocking, since a completed meeting is already over.
