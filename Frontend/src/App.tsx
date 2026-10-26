import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import RoomList from './components/RoomList'
import UserList from './components/UserList'
import ReservationList from './components/ReservationList'

function App() {
  return (
    <BrowserRouter>
    <nav className="navbar navbar-expand navbar-dark bg-dark px-3">
      <div className="navbar-nav">
      <Link className="nav-link text-white" to="/rooms">Rooms</Link>
      <Link className="nav-link text-white" to="/users">Users</Link>
      <Link className="nav-link text-white" to="/reservations">Reservations</Link>
      </div>
      </nav>
      <div className="container mt-3">
        <Routes>
          <Route path="/rooms" element={<RoomList/>}/>
          <Route path="/users" element={<UserList/>}/>
          <Route path="/reservations" element={<ReservationList/>}/>
        </Routes>
      </div>
      </BrowserRouter>
  )
}

export default App
