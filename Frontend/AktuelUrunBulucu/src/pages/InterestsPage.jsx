import { useState, useEffect } from 'react'

const STORAGE_KEY = 'aktuel_interests'

const SUGGESTED_INTERESTS = [
  'Spor', 'Kamp', 'Elektronik', 'Bahçe', 'Mobilya',
  'Giyim', 'Çocuk', 'Beyaz Eşya', 'Yaz', 'Bisiklet',
]

export default function InterestsPage({ onNavigate }) {
  const [interests, setInterests] = useState([])
  const [input, setInput] = useState('')

  useEffect(() => {
    try {
      const saved = JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]')
      setInterests(saved)
    } catch {
      setInterests([])
    }
  }, [])

  function save(updated) {
    setInterests(updated)
    localStorage.setItem(STORAGE_KEY, JSON.stringify(updated))
  }

  function addInterest() {
    const trimmed = input.trim()
    if (!trimmed) return
    if (interests.includes(trimmed)) {
      setInput('')
      return
    }
    save([...interests, trimmed])
    setInput('')
  }

  function removeInterest(item) {
    save(interests.filter(i => i !== item))
  }

  function handleKeyDown(e) {
    if (e.key === 'Enter') addInterest()
  }

  function addSuggested(s) {
    if (!interests.includes(s)) save([...interests, s])
  }

  return (
    <div style={{ maxWidth: '600px', margin: '0 auto', padding: '24px 16px' }}>
      <h2 style={{ fontSize: '20px', fontWeight: 700, color: '#111827', marginBottom: '6px' }}>
        İlgi Alanlarım
      </h2>
      <p style={{ fontSize: '13px', color: '#6b7280', marginBottom: '24px' }}>
        İlgi alanlarınızı ekleyin. Ürün önerileri bu alanlara göre hazırlanacaktır.
      </p>

      {/* Input */}
      <div style={{ display: 'flex', gap: '8px', marginBottom: '16px' }}>
        <input
          value={input}
          onChange={e => setInput(e.target.value)}
          onKeyDown={handleKeyDown}
          placeholder="Örn: Spor, Kamp, Elektronik..."
          style={{
            flex: 1,
            padding: '10px 14px',
            border: '1px solid #d1d5db',
            borderRadius: '8px',
            fontSize: '14px',
            outline: 'none',
          }}
        />
        <button
          onClick={addInterest}
          disabled={!input.trim()}
          style={{
            padding: '10px 20px',
            background: input.trim() ? '#374151' : '#9ca3af',
            color: '#fff',
            border: 'none',
            borderRadius: '8px',
            fontSize: '14px',
            fontWeight: 600,
            cursor: input.trim() ? 'pointer' : 'not-allowed',
            transition: 'background 0.15s',
          }}
        >
          Ekle
        </button>
      </div>

      {/* Eklenen ilgi alanları */}
      {interests.length > 0 && (
        <div style={{ marginBottom: '24px' }}>
          <div style={{ fontSize: '12px', fontWeight: 600, color: '#374151', marginBottom: '8px', textTransform: 'uppercase', letterSpacing: '0.5px' }}>
            Eklenen İlgi Alanları
          </div>
          <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px' }}>
            {interests.map(item => (
              <span
                key={item}
                style={{
                  display: 'inline-flex',
                  alignItems: 'center',
                  gap: '6px',
                  padding: '6px 12px',
                  background: '#374151',
                  color: '#fff',
                  borderRadius: '9999px',
                  fontSize: '13px',
                  fontWeight: 500,
                }}
              >
                {item}
                <button
                  onClick={() => removeInterest(item)}
                  style={{
                    background: 'rgba(255,255,255,0.25)',
                    border: 'none',
                    borderRadius: '50%',
                    width: '18px',
                    height: '18px',
                    color: '#fff',
                    fontSize: '11px',
                    cursor: 'pointer',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    lineHeight: 1,
                    padding: 0,
                  }}
                >✕</button>
              </span>
            ))}
          </div>
        </div>
      )}

      {/* Önerilen kategoriler */}
      <div style={{ marginBottom: '32px' }}>
        <div style={{ fontSize: '12px', fontWeight: 600, color: '#374151', marginBottom: '8px', textTransform: 'uppercase', letterSpacing: '0.5px' }}>
          Hızlı Ekle
        </div>
        <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px' }}>
          {SUGGESTED_INTERESTS.map(s => {
            const added = interests.includes(s)
            return (
              <button
                key={s}
                onClick={() => !added && addSuggested(s)}
                style={{
                  padding: '6px 14px',
                  background: added ? '#e5e7eb' : '#f3f4f6',
                  color: added ? '#9ca3af' : '#374151',
                  border: `1px solid ${added ? '#d1d5db' : '#e5e7eb'}`,
                  borderRadius: '9999px',
                  fontSize: '13px',
                  cursor: added ? 'default' : 'pointer',
                  transition: 'background 0.15s',
                }}
              >
                {added ? '✓ ' : '+ '}{s}
              </button>
            )
          })}
        </div>
      </div>

      {/* CTA */}
      <button
        onClick={() => onNavigate('recommendations')}
        disabled={interests.length === 0}
        style={{
          width: '100%',
          padding: '14px',
          background: interests.length > 0 ? '#2563eb' : '#9ca3af',
          color: '#fff',
          border: 'none',
          borderRadius: '10px',
          fontSize: '15px',
          fontWeight: 700,
          cursor: interests.length > 0 ? 'pointer' : 'not-allowed',
          transition: 'background 0.15s',
        }}
      >
        Önerileri Gör ({interests.length} ilgi alanı)
      </button>
    </div>
  )
}
