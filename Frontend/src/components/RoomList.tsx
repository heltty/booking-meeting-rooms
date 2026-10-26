import {useEffect, useState} from 'react'
import type { Room } from '../types/room'

function RoomList() {
    const [rooms, setRooms] = useState<Room[]>([])
    const [minCapacity, setMinCapacity] = useState('')
    const [matchingFloor, setMatchingFloor] = useState('')
    const [matchingEquipment, setMatchingEquipment]=useState('')

    useEffect(()=>{
        const params = new URLSearchParams()
        if(minCapacity) params.append('minCapacity', minCapacity)
        if(matchingFloor) params.append('matchingFloor', matchingFloor)
        if(matchingEquipment) params.append('matchingEquipment', matchingEquipment)

        fetch(`http://localhost:5241/rooms?${params.toString()}`)
        .then(response=>response.json())
        .then(data=>setRooms(data))
    }, [minCapacity, matchingFloor, matchingEquipment])
    return (
        <>
            <select
            className="form-select"
            value={matchingEquipment}
            onChange={e=>setMatchingEquipment(e.target.value)}>
            <option value="">All equipment</option>
            <option value="Projector">Projector</option>
            <option value="TVscreen">TV screen</option>
            <option value="VideoConferencing">Video Conferencing</option>
            <option value="Whiteboard">Whiteboard</option>
            </select>
            <input
            type="text"
            className="form-control"
            placeholder="Matching floor"
            value={matchingFloor}
            onChange={e=>setMatchingFloor(e.target.value)}
            />
            <input
            type="number"
            className="form-control"
            placeholder="Minimum capacity"
            value={minCapacity}
            onChange={e=>setMinCapacity(e.target.value)}
            />        
        <table className="table table-striped">
            <thead>
                <tr>
                    <th>Room name</th>
                    <th>Room id</th>
                    <th>Room capacity</th>
                    <th>Room floor</th>
                    <th>Room equipment</th>
                    <th>Room status</th>
                </tr>
            </thead>
            <tbody>
                {rooms.map(room=>(
                    <tr key={room.id}>
                        <td>{room.name}</td>
                        <td>{room.id}</td>
                        <td>{room.capacity}</td>
                        <td>{room.floor}</td>
                        <td>{room.equipment.join(', ')}</td>
                        <td>{room.isActive ? 'Active' : 'Inactive'}</td>
                    </tr>
                ))}
            </tbody>
        </table>
        </>
    );
}

export default RoomList