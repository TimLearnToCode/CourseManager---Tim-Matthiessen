import { BrowserRouter, Routes, Route, NavLink } from 'react-router-dom'
import CoursesPage from './pages/CoursesPage'
import TeachersPage from './pages/TeachersPage'
import ParticipantsPage from './pages/ParticipantsPage'

export default function App() {
  return (
    <BrowserRouter>
      <nav>
        <span className="brand">📚 Courses Manager</span>
        <NavLink to="/">Kurser</NavLink>
        <NavLink to="/teachers">Lärare</NavLink>
        <NavLink to="/participants">Deltagare</NavLink>
      </nav>
      <div className="container">
        <Routes>
          <Route path="/" element={<CoursesPage />} />
          <Route path="/teachers" element={<TeachersPage />} />
          <Route path="/participants" element={<ParticipantsPage />} />
        </Routes>
      </div>
    </BrowserRouter>
  )
}
