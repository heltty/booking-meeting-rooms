import { useState } from "react";
import type { Room } from "../types/room";
import type { User } from "../types/user";

type ReservationFormProps = {
    rooms: Room[]
    users: User[]
    onCreated: () => void
}

function ReservationForm({rooms, users, onCreated}: ReservationFormProps) {
    const[meetingTitle, setMeetingTitle] = useState('')
    const[attendees, setAttendees] = useState('')
    const[startDate, setStartDate] = useState('')
    const[endDate, setEndDate] = useState('')
    const[selectedRoom, setSelectedRoom] = useState('')
    const[selectedUser, setSelectedUser] = useState('')
    const[error, setError] = useState('')

    const handleCreate = () => {
        if(!startDate){
            setError('Please choose reservation start date')
            return
        }
        if(!endDate){
            setError('Please choose reservation end date')
            return
        }
        if(!attendees || Number(attendees) <= 0 || !Number.isInteger(Number(attendees))){
            setError('Please specify attendee count')
            return
        }
        if(!selectedRoom){
            setError("Please choose a room")
            return
        }
        if(!selectedUser){
            setError("Please choose the organizer")
            return
        }
        if(!meetingTitle.trim()){
            setError("Please fill the meeting title")
            return
        }
        const request = {
            roomId: Number(selectedRoom),
            organizerId: Number(selectedUser),
            meetingTitle,
            attendees: Number(attendees),
            start: new Date(startDate).toISOString(),
            end: new Date(endDate).toISOString()

        }
        fetch('http://localhost:5241/reservations',{
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        }).then(async response =>{
            if(!response.ok){
                const err=await response.json()
                setError(err.message)
                return
            }
            setError('')
            onCreated()
        })
    }

    return (
        <div className="row mb-3">
            <div className="col">
                <label className="form-label">Select a room</label>
                <select
                className="form-select"
                value={selectedRoom}
                onChange={e=>setSelectedRoom(e.target.value)}>
                    <option value="">Select a room</option>
                    {rooms.map(r=>(
                        <option key={r.id} value={r.id}>
                            {r.name}
                        </option>
                    ))}
                </select>
            </div>
            <div className="col">
                <label className="form-label">Select the organizer</label>
                <select
                className="form-select"
                value={selectedUser}
                onChange={e=>setSelectedUser(e.target.value)}>
                    <option value="">Select the organizer</option>
                    {users.map(r=>(
                        <option key={r.id} value={r.id}>
                            {r.firstName} {r.lastName}
                        </option>
                    ))}
                </select>
            </div>
            <div className="col">
                <label className="form-label">Meeting title</label>
                <input
                    type="text"
                    className="form-control"
                    value={meetingTitle}
                    onChange={e=> setMeetingTitle(e.target.value)}
                    />
            </div>
            <div className="col">
                <label className="form-label">Attendee count</label>
                <input
                type="number"
                className="form-control"
                value={attendees}
                onChange={e=> setAttendees(e.target.value)}
                />
            </div>
            <div className="col">
                <label className="form-label">Start date</label>
                <input
                type="datetime-local"
                className="form-control"
                value={startDate}
                onChange={e=> setStartDate(e.target.value)}
                />
            </div>
            <div className="col">
                <label className="form-label">End date</label>
                <input
                type="datetime-local"
                className="form-control"
                value={endDate}
                onChange={e=> setEndDate(e.target.value)}
                />
            </div>
            <div className="col-12 mt-3">
                {error && <div className="alert alert-danger">{error}</div>}
                <button type="button" className="btn btn-sm btn-success"
                    onClick={handleCreate}>
                    Create
                </button>
            </div>
        </div>
    )
}

export default ReservationForm