# booking-meeting-rooms

## About the project

## Technology used

## Architecture decisions

1. Users page (Backend)
- Chose enum for equipment variables, since it's a fixed variety of items (Projector, TV screen, ...), this way in case there's a need to add or update said item instead of writing the item with a chance of miss-spelling it, enum simply holds int values starting from 0. Also, a room can have several of those items, so I will store Equipment in a List.
2. Users page (Backend)
- "How many active reservations, user has" - User.cs model class doesn't store that value, because it would need constant updating, I simply do it on the Reservations page.
3. Reservations page (Backend)
- Chose enum for reservation status variable, since there's 3 possible choices.
- Chose int for Room and Organizer variables, because in case there's a need to rename a room it would cause issues if the variable was a string, by making RoomId and OrganizerId int, i have the ability to track which room is which by a number and simply change it in Room.cs (1.Users page)