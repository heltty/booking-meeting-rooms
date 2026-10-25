import {useEffect, useState} from 'react'
import type {User} from '../types/user'

function UserList() {
    const[users, setUsers] = useState<User[]>([])
    
    useEffect(()=>{
        fetch('http://localhost:5241/users')
        .then(response=>response.json())
        .then(data=>setUsers(data))
    }, [])

    return (
        <table className="table table-striped">
            <thead>
                <tr>
                    <th>User id</th>
                    <th>User name</th>
                    <th>User reservation count</th>
                </tr>
            </thead>
            <tbody>
                {users.map(user=>(
                    <tr key={user.id}>
                        <td>{user.id}</td>
                        <td>{user.firstName} {user.lastName}</td>
                        <td>{user.activeReservations}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    )
}
export default UserList