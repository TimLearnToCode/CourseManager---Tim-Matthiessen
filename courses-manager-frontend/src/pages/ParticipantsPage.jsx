import { useState, useEffect } from 'react'
import { getParticipants, createParticipant, updateParticipant, deleteParticipant } from '../api/client'

function ParticipantModal({ participant, onClose, onSaved }) {
  const isEdit = !!participant
  const [form, setForm] = useState({
    firstName: participant?.firstName ?? '',
    lastName: participant?.lastName ?? '',
    email: participant?.email ?? '',
  })
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const handle = (e) => setForm({ ...form, [e.target.name]: e.target.value })

  const submit = async () => {
    setError('')
    setLoading(true)
    try {
      if (isEdit) {
        await updateParticipant(participant.id, form)
      } else {
        await createParticipant(form)
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
        <h2>{isEdit ? 'Redigera deltagare' : 'Ny deltagare'}</h2>
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

export default function ParticipantsPage() {
  const [participants, setParticipants] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [modal, setModal] = useState(null)

  const load = async () => {
    setLoading(true)
    try {
      const data = await getParticipants()
      setParticipants(data)
    } catch (e) {
      setError(e.message)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => { load() }, [])

  const handleDelete = async (id, name) => {
    if (!confirm(`Ta bort deltagaren ${name}?`)) return
    try {
      await deleteParticipant(id)
      load()
    } catch (e) {
      alert(e.message)
    }
  }

  return (
    <div>
      <div className="top-bar">
        <h1>Deltagare</h1>
        <button className="btn btn-primary" onClick={() => setModal('create')}>+ Ny deltagare</button>
      </div>

      {error && <div className="error-msg">{error}</div>}
      {loading && <p className="loading">Laddar...</p>}

      {!loading && participants.length === 0 && (
        <p className="empty">Inga deltagare hittades. Lägg till en ny!</p>
      )}

      {participants.map((p) => (
        <div className="card" key={p.id}>
          <div className="card-info">
            <h3>{p.firstName} {p.lastName}</h3>
            <p>{p.email}</p>
          </div>
          <div className="btn-group">
            <button className="btn btn-primary" onClick={() => setModal(p)}>Redigera</button>
            <button className="btn btn-danger" onClick={() => handleDelete(p.id, `${p.firstName} ${p.lastName}`)}>Ta bort</button>
          </div>
        </div>
      ))}

      {modal && (
        <ParticipantModal
          participant={modal === 'create' ? null : modal}
          onClose={() => setModal(null)}
          onSaved={() => { setModal(null); load() }}
        />
      )}
    </div>
  )
}
