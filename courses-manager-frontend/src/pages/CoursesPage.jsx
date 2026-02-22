import { useState, useEffect } from 'react'
import { getCourses, createCourse, updateCourse, deleteCourse } from '../api/client'

function CourseModal({ course, onClose, onSaved }) {
  const isEdit = !!course
  const [form, setForm] = useState({
    courseCode: course?.courseCode ?? '',
    title: course?.title ?? '',
    description: course?.description ?? '',
  })
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const handle = (e) => setForm({ ...form, [e.target.name]: e.target.value })

  const submit = async () => {
    setError('')
    setLoading(true)
    try {
      if (isEdit) {
        const dto = { title: form.title, description: form.description, rowVersion: course.rowVersion }
        await updateCourse(course.courseCode, dto)
      } else {
        await createCourse(form)
      }
      onSaved()
    } catch (e) {
      setError(e.message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal" onClick={(e) => e.stopPropagation()}>
        <h2>{isEdit ? 'Redigera kurs' : 'Ny kurs'}</h2>
        {error && <div className="error-msg">{error}</div>}
        {!isEdit && (
          <div className="form-group">
            <label>Kurskod</label>
            <input name="courseCode" value={form.courseCode} onChange={handle} placeholder="t.ex. CS101" />
          </div>
        )}
        <div className="form-group">
          <label>Titel</label>
          <input name="title" value={form.title} onChange={handle} placeholder="Kursens titel" />
        </div>
        <div className="form-group">
          <label>Beskrivning</label>
          <textarea name="description" value={form.description} onChange={handle} placeholder="Beskrivning av kursen" />
        </div>
        <div className="modal-actions">
          <button className="btn btn-secondary" onClick={onClose}>Avbryt</button>
          <button className="btn btn-primary" onClick={submit} disabled={loading}>
            {loading ? 'Sparar...' : 'Spara'}
          </button>
        </div>
      </div>
    </div>
  )
}

export default function CoursesPage() {
  const [courses, setCourses] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [modal, setModal] = useState(null) // null | 'create' | courseObject

  const load = async () => {
    setLoading(true)
    try {
      const data = await getCourses()
      setCourses(data)
    } catch (e) {
      setError(e.message)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { load() }, [])

  const handleDelete = async (courseCode) => {
    if (!confirm(`Ta bort kursen ${courseCode}?`)) return
    try {
      await deleteCourse(courseCode)
      load()
    } catch (e) {
      alert(e.message)
    }
  }

  return (
    <div>
      <div className="top-bar">
        <h1>Kurser</h1>
        <button className="btn btn-primary" onClick={() => setModal('create')}>+ Ny kurs</button>
      </div>

      {error && <div className="error-msg">{error}</div>}
      {loading && <p className="loading">Laddar...</p>}

      {!loading && courses.length === 0 && (
        <p className="empty">Inga kurser hittades. Skapa en ny kurs!</p>
      )}

      {courses.map((c) => (
        <div className="card" key={c.courseCode}>
          <div className="card-info">
            <h3>{c.title} <span style={{ color: '#999', fontWeight: 400 }}>({c.courseCode})</span></h3>
            <p>{c.description}</p>
          </div>
          <div className="btn-group">
            <button className="btn btn-primary" onClick={() => setModal(c)}>Redigera</button>
            <button className="btn btn-danger" onClick={() => handleDelete(c.courseCode)}>Ta bort</button>
          </div>
        </div>
      ))}

      {modal && (
        <CourseModal
          course={modal === 'create' ? null : modal}
          onClose={() => setModal(null)}
          onSaved={() => { setModal(null); load() }}
        />
      )}
    </div>
  )
}
