# booking-meeting-rooms

## About the project

## Technology used

## Architecture decisions

1. Users page (Backend)
- Chose enum for equipment variables, since it's a fixed variety of items (Projector, TV screen, ...), this way in case there's a need to add or update said item instead of writing the item with a chance of misspelling it, enum simply holds int values starting from 0. Also, a room can have several of those items, so I will store Equipment in a List.
2. Users page (Backend)
- "How many active reservations, user has" - User.cs model class doesn't store that value, because it would need constant updating, I simply do it on the Reservations page.
3. Reservations page (Backend)
- Chose enum for reservation status variable, since there's 3 possible choices.
- Chose int for Room and Organizer variables, because in case there's a need to rename a room it would cause issues if the variable was a string, by making RoomId and OrganizerId int, I have the ability to track which room is which by a number and simply change it in Room.cs (1.Users page)
***
- Added database seeders, since this database is in-memory, meaning, that once the program is shut down every database element disappears. Having a seeder lets me test the program anytime.
***
- There are two status variables in different models (one being in Room.cs and the other in Reservation.cs), the thing that differs them is:
  - Room.IsActive is for a room, either true or false, this simply means if the room is even up for reservation (in case of a renovation would make it false)
  - Reservation.Status is the reservation status (Booked, Cancelled, Completed). Rule 2 says that active reservations are those that are still happening, that means Booked.