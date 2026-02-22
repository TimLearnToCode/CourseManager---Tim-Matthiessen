import { useState, useEffect } from 'react'
import { getTeachers, createTeacher, updateTeacher, deleteTeacher } from '../api/client'

function TeacherModal({ teacher, onClose, onSaved }) {
  const isEdit = !!teacher
  const [form, setForm] = useState({
    firstName: teacher?.firstName ?? '',
    lastName: teacher?.lastName ?? '',
    email: teacher?.email ?? '',
  })
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const handle = (e) => setForm({ ...form, [e.target.name]: e.target.value })

  const submit = async () => {
    setError('')
    setLoading(true)
    try {
      if (isEdit) {
        await updateTeacher(teacher.id, form)
      } else {
        await createTeacher(form)
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
        <h2>{isEdit ? 'Redigera lärare' : 'Ny lärare'}</h2>
        {error && <div className="error-msg">{error}</div>}
        <div className="form-group">
          <label>Förnamn</label>
          <input name="firstName" value={form.firstName} onChange={handle} placeholder="Förnamn" />
        </div>
        <div className="form-group">
          <label>Efternamn</label>
          <input name="lastName" value={form.lastName} onChange={handle} placeholder="Efternamn" />
        </div>
        <div className="form-group">
          <label>E-post</label>
          <input name="email" value={form.email} onChange={handle} placeholder="email@exempel.se" type="email" />
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

export default function TeachersPage() {
  const [teachers, setTeachers] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [modal, setModal] = useState(null)

  const load = async () => {
    setLoading(true)
    try {
      const data = await getTeachers()
      setTeachers(data)
    } catch (e) {
      setError(e.message)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { load() }, [])

  const handleDelete = async (id, name) => {
    if (!confirm(`Ta bort läraren ${name}?`)) return
    try {
      await deleteTeacher(id)
      load()
    } catch (e) {
      alert(e.message)
    }
  }

  return (
    <div>
      <div className="top-bar">
        <h1>Lärare</h1>
        <button className="btn btn-primary" onClick={() => setModal('create')}>+ Ny lärare</button>
      </div>

      {error && <div className="error-msg">{error}</div>}
      {loading && <p className="loading">Laddar...</p>}

      {!loading && teachers.length === 0 && (
        <p className="empty">Inga lärare hittades. Lägg till en ny!</p>
      )}

      {teachers.map((t) => (
        <div className="card" key={t.id}>
          <div className="card-info">
            <h3>{t.firstName} {t.lastName}</h3>
            <p>{t.email}</p>
          </div>
          <div className="btn-group">
            <button className="btn btn-primary" onClick={() => setModal(t)}>Redigera</button>
            <button className="btn btn-danger" onClick={() => handleDelete(t.id, `${t.firstName} ${t.lastName}`)}>Ta bort</button>
          </div>
        </div>
      ))}

      {modal && (
        <TeacherModal
          teacher={modal === 'create' ? null : modal}
          onClose={() => setModal(null)}
          onSaved={() => { setModal(null); load() }}
        />
      )}
    </div>
  )
}
