const BASE_URL = '/api'

async function request(path, options = {}) {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json', ...options.headers },
    ...options,
  })
  if (res.status === 204) return null
  const data = await res.json()
  if (!res.ok) throw new Error(data?.detail || data?.title || 'Något gick fel')
  return data
}

// Courses
export const getCourses = () => request('/courses')
export const getCourse = (code) => request(`/courses/${code}`)
export const createCourse = (dto) => request('/courses', { method: 'POST', body: JSON.stringify(dto) })
export const updateCourse = (code, dto) => request(`/courses/${code}`, { method: 'PUT', body: JSON.stringify(dto) })
export const deleteCourse = (code) => request(`/courses/${code}`, { method: 'DELETE' })

// Teachers
export const getTeachers = () => request('/teachers')
export const createTeacher = (dto) => request('/teachers', { method: 'POST', body: JSON.stringify(dto) })
export const updateTeacher = (id, dto) => request(`/teachers/${id}`, { method: 'PUT', body: JSON.stringify(dto) })
export const deleteTeacher = (id) => request(`/teachers/${id}`, { method: 'DELETE' })

// Participants
export const getParticipants = () => request('/participants')
export const createParticipant = (dto) => request('/participants', { method: 'POST', body: JSON.stringify(dto) })
export const updateParticipant = (id, dto) => request(`/participants/${id}`, { method: 'PUT', body: JSON.stringify(dto) })
export const deleteParticipant = (id) => request(`/participants/${id}`, { method: 'DELETE' })

// Registrations
export const getRegistrationsForSession = (sessionId) => request(`/registrations/session/${sessionId}`)
export const createRegistration = (dto) => request('/registrations', { method: 'POST', body: JSON.stringify(dto) })
export const deleteRegistration = (id) => request(`/registrations/${id}`, { method: 'DELETE' })
