import { useEffect, useState } from "react";
import type { Reservation } from "../types/reservation";
import type { Room } from "../types/room";
import type { User } from "../types/user";

function ReservationList() {
    const[reservations, setReservations] = useState<Reservation[]>([])
    const[rooms, setRooms] = useState<Room[]>([])
    const[users, setUsers] = useState<User[]>([])

    useEffect(()=>{
        fetch('http://localhost:5241/users')
        .then(response=>response.json())
        .then(data=>setUsers(data))

        fetch('http://localhost:5241/rooms')
        .then(response=>response.json())
        .then(data=>setRooms(data))

        fetch('http://localhost:5241/reservations')
        .then(response=>response.json())
        .then(data=>setReservations(data))
    },[]);

    const getRoomName = (roomId: number) =>{
        const room = rooms.find(r=>r.id===roomId)
        return room ? room.name : null;
    };
    const getOrganizerName = (organizerId: number)=>{
        const organizer = users.find(o=>o.id===organizerId)
        return organizer ? organizer.firstName+ " " + organizer.lastName : null;
    }

    return (
        <table className="table table-striped">
            <thead>
                <tr>
                    <th>Room name</th>
                    <th>Organizer name</th>
                    <th>Meeting title</th>
                    <th>Reservation start date</th>
                    <th>Reservation end date</th>
                    <th>Number of attendees</th>
                    <th>Reservation status</th>
                </tr>
            </thead>
            <tbody>
                {reservations.map(reservation=>(
                    <tr key={reservation.id}>
                        <td>{getRoomName(reservation.roomId)}</td>
                        <td>{getOrganizerName(reservation.organizerId)}</td>
                        <td>{reservation.meetingTitle}</td>
                        <td>{reservation.start}</td>
                        <td>{reservation.end}</td>
                        <td>{reservation.attendees}</td>
                        <td>{reservation.status}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}

export default ReservationList