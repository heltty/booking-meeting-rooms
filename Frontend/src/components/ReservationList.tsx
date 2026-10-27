import { useEffect, useState } from "react";
import type { Reservation } from "../types/reservation";
import type { Room } from "../types/room";
import type { User } from "../types/user";
import ReservationForm from "./ReservationForm";

function ReservationList() {
    const[reservations, setReservations] = useState<Reservation[]>([])
    const[rooms, setRooms] = useState<Room[]>([])
    const[users, setUsers] = useState<User[]>([])
    const[matchingStatus, setMatchingStatus] = useState('')
    const[matchingRoom, setMatchingRoom] = useState('')
    const[matchingOrganizer, setMatchingOrganizer] = useState('')
    const[dateRangeStart, setDateRangeStart] = useState('')
    const[dateRangeEnd, setDateRangeEnd] = useState('')
    const[reload, setReload]=useState(0)
    const[error, setError]=useState('')
    const[showForm, setShowForm]=useState(false)

    useEffect(()=>{
        fetch('http://localhost:5241/users')
        .then(response=>response.json())
        .then(data=>setUsers(data))

        fetch('http://localhost:5241/rooms')
        .then(response=>response.json())
        .then(data=>setRooms(data))
    },[]);

    useEffect(()=>{
        const params = new URLSearchParams()
        if(matchingStatus) params.append('matchingStatus', matchingStatus);
        if(matchingRoom) params.append('matchingRoom', matchingRoom);
        if(matchingOrganizer) params.append('matchingOrganizer', matchingOrganizer);
        if(dateRangeStart) params.append('dateRangeStart', dateRangeStart);
        if(dateRangeEnd) params.append('dateRangeEnd', dateRangeEnd);

        fetch(`http://localhost:5241/reservations?${params.toString()}`)
        .then(response=>response.json())
        .then(data=>setReservations(data))
    }, [matchingRoom, matchingStatus, matchingOrganizer, dateRangeStart, dateRangeEnd, reload]);


    const cancel=(id:number)=>{
        fetch(`http://localhost:5241/reservations/${id}/cancel`, {method: 'PATCH'})
        .then(async response=>{
            if(!response.ok){
                const err=await response.json()
                setError(err.message)
                return
            }
            setError('')
            setReload(reload + 1)
        })
    }
    const complete=(id:number)=>{
        fetch(`http://localhost:5241/reservations/${id}/complete`, {method: 'PATCH'})
        .then(async response=>{
            if(!response.ok){
                const err=await response.json()
                setError(err.message)
                return
            }
            setError('')
            setReload(reload + 1)
        })
    }
    const handleReservationCreated = () => {
        setShowForm(false)
        setReload(prev=> prev + 1)
    }
    const getRoomName = (roomId: number) =>{
        const room = rooms.find(r=>r.id===roomId)
        return room ? room.name : null;
    };
    const getOrganizerName = (organizerId: number)=>{
        const organizer = users.find(o=>o.id===organizerId)
        return organizer ? organizer.firstName+ " " + organizer.lastName : null;
    };
    const formatDate =(value: string) =>
        new Date(value).toLocaleString('lt-LT', {dateStyle: 'short', timeStyle:'short'})

    return (
        <>
        <button className="btn btn-sm btn-success"
                    onClick={()=> setShowForm(!showForm)}
                    >
                        {!showForm ? "Create reservation" : "Back to reservations"}
                </button>
                {showForm && <ReservationForm rooms={rooms} users={users} onCreated={handleReservationCreated}/>}
            {!showForm && (
            <>
        <div className="row mb-3">
            <div className="col">
                <label className="form-label small fw-semibold text-body-secondary">Filter by start date</label>
                <input
                type="date"
                className="form-control"
                value={dateRangeStart}
                onChange={e=>setDateRangeStart(e.target.value)}
                />
            </div>
            <div className="col">
                <label className="form-label small fw-semibold text-body-secondary">Filter by end date</label>
                <input
                type="date"
                className="form-control"
                value={dateRangeEnd}
                onChange={e=>setDateRangeEnd(e.target.value)}
                />
            </div>
        </div>
        <div className="row mb-3">
            <div className="col">
                <label className="form-label small fw-semibold text-body-secondary">Filter by room name</label>
                <select
                className="form-select"
                value={matchingRoom}
                onChange={e=>setMatchingRoom(e.target.value)}>
                <option value="">All rooms</option>
                {rooms.map(r=>(
                    <option key={r.id} value={r.id}>{r.name}</option>
                ))}
                </select>
            </div>
            <div className="col">
                <label className="form-label small fw-semibold text-body-secondary">Filter by organizer name</label>
                <select
                className="form-select"
                value={matchingOrganizer}
                onChange={e=>setMatchingOrganizer(e.target.value)}>
                <option value="">All organizators</option>
                {users.map(r=>(
                    <option key={r.id} value={r.id}>{r.firstName} {r.lastName}</option>
                ))}
                </select>
            </div>
            <div className="col">
                <label className="form-label small fw-semibold text-body-secondary">Filter by reservation status</label>
                <select
                className="form-select"
                value={matchingStatus}
                onChange={e=>setMatchingStatus(e.target.value)}>
                <option value="">All status</option>
                <option value="Booked">Booked</option>
                <option value="Cancelled">Cancelled</option>
                <option value="Completed">Completed</option>
                </select>
            </div>
        </div>
        {error && <div className="alert alert-danger">{error}</div>}
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
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {reservations.map(reservation=>(
                    <tr key={reservation.id}>
                        <td>{getRoomName(reservation.roomId)}</td>
                        <td>{getOrganizerName(reservation.organizerId)}</td>
                        <td>{reservation.meetingTitle}</td>
                        <td>{formatDate(reservation.start)}</td>
                        <td>{formatDate(reservation.end)}</td>
                        <td>{reservation.attendees}</td>
                        <td>{reservation.status}</td>
                        <td>{reservation.status === "Booked" &&(<button 
                            className="btn btn-sm btn-danger"
                            onClick={() => cancel(reservation.id)}
                            >
                                Cancel
                            </button>
                        )}
                            {reservation.status === "Booked" && (<button
                                className="btn btn-sm btn-success"
                                onClick={()=> complete(reservation.id)}
                                >
                                    Complete
                                </button>
                            )}
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
        </>
        )}
        </>
    );
}

export default ReservationList