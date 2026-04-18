import { useState, useEffect } from 'react'
import { Loader } from '@progress/kendo-react-indicators'
import { getRecommendations } from '../api/recommendationApi'

const STORAGE_KEY = 'aktuel_interests'
const STALE_THRESHOLD_DAYS = 7

const SUGGESTED_INTERESTS = [
  'Spor', 'Kamp', 'Elektronik', 'Bahçe', 'Mobilya',
  'Giyim', 'Çocuk', 'Beyaz Eşya', 'Yaz', 'Bisiklet',
  'Fitness', 'Yüzme', 'Trekking', 'Piknik', 'Balıkçılık',
  'Fotoğrafçılık', 'Oyun', 'Mutfak', 'Temizlik', 'Dekorasyon',
  'Araba', 'Araç Gereç', 'Tamirat', 'Okul', 'Kitap',
  'Müzik', 'Seyahat', 'Evcil Hayvan', 'Kozmetik', 'Sağlık',
]

const CHAIN_COLORS = {
  Migros:      '#f97316',
  A101:        '#ef4444',
  'BİM':       '#3b82f6',
  'Şok':       '#8b5cf6',
  CarrefourSA: '#10b981',
}

function chainColor(name) {
  return CHAIN_COLORS[name] || '#6b7280'
}

function isStale(date) {
  if (!date) return false
  return Date.now() - new Date(date).getTime() > STALE_THRESHOLD_DAYS * 86400_000
}

export default function InterestsPage() {
  const [interests, setInterests] = useState([])
  const [input, setInput] = useState('')
  const [results, setResults] = useState([])
  const [recLoading, setRecLoading] = useState(false)

  /* localStorage'dan yükle */
  useEffect(() => {
    try {
      const saved = JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]')
      setInterests(saved)
    } catch {
      setInterests([])
    }
  }, [])

  /* ilgi alanları değiştiğinde önerileri getir */
  useEffect(() => {
    if (interests.length === 0) {
      setResults([])
      return
    }
    setRecLoading(true)
    getRecommendations(interests)
      .then(data => setResults(data.results || []))
      .catch(() => setResults([]))
      .finally(() => setRecLoading(false))
  }, [interests])

  function save(updated) {
    setInterests(updated)
    localStorage.setItem(STORAGE_KEY, JSON.stringify(updated))
  }

  function addInterest() {
    const trimmed = input.trim()
    if (!trimmed || interests.includes(trimmed)) { setInput(''); return }
    save([...interests, trimmed])
    setInput('')
  }

  function removeInterest(item) { save(interests.filter(i => i !== item)) }

  function addSuggested(s) { if (!interests.includes(s)) save([...interests, s]) }

  const totalProducts = results.reduce((sum, r) => sum + r.products.length, 0)

  return (
    <div style={{ maxWidth: '680px', margin: '0 auto', padding: '24px 16px' }}>

      {/* ── Form ─────────────────────────────────────── */}
      <h2 style={{ fontSize: '18px', fontWeight: 700, color: '#111827', marginBottom: '4px' }}>
        İlgi Alanlarım
      </h2>
      <p style={{ fontSize: '13px', color: '#6b7280', marginBottom: '20px' }}>
        Hobinizi yazın — aşağıda eşleşen aktüel ürünler otomatik listelenir.
      </p>

      {/* Input */}
      <div style={{ display: 'flex', gap: '8px', marginBottom: '14px' }}>
        <input
          value={input}
          onChange={e => setInput(e.target.value)}
          onKeyDown={e => e.key === 'Enter' && addInterest()}
          placeholder="Örn: Kamp, Bisiklet, Elektronik..."
          style={{
            flex: 1, padding: '10px 14px',
            border: '1px solid #d1d5db', borderRadius: '8px',
            fontSize: '14px', outline: 'none',
          }}
        />
        <button
          onClick={addInterest}
          disabled={!input.trim()}
          style={{
            padding: '10px 20px',
            background: input.trim() ? '#374151' : '#9ca3af',
            color: '#fff', border: 'none', borderRadius: '8px',
            fontSize: '14px', fontWeight: 600,
            cursor: input.trim() ? 'pointer' : 'not-allowed',
            transition: 'background 0.15s',
          }}
        >
          Ekle
        </button>
      </div>

      {/* Eklenen etiketler */}
      {interests.length > 0 && (
        <div style={{ marginBottom: '16px' }}>
          <div style={{ fontSize: '11px', fontWeight: 600, color: '#6b7280', marginBottom: '8px', textTransform: 'uppercase', letterSpacing: '0.5px' }}>
            Seçilen İlgi Alanları
          </div>
          <div style={{ display: 'flex', flexWrap: 'wrap', gap: '8px' }}>
            {interests.map(item => (
              <span key={item} style={{
                display: 'inline-flex', alignItems: 'center', gap: '6px',
                padding: '5px 12px', background: '#374151', color: '#fff',
                borderRadius: '9999px', fontSize: '13px', fontWeight: 500,
              }}>
                {item}
                <button
                  onClick={() => removeInterest(item)}
                  style={{
                    background: 'rgba(255,255,255,0.25)', border: 'none',
                    borderRadius: '50%', width: '16px', height: '16px',
                    color: '#fff', fontSize: '10px', cursor: 'pointer',
                    display: 'flex', alignItems: 'center', justifyContent: 'center',
                    lineHeight: 1, padding: 0,
                  }}
                >✕</button>
              </span>
            ))}
          </div>
        </div>
      )}

      {/* Hızlı ekle */}
      <div style={{ marginBottom: '28px' }}>
        <div style={{ fontSize: '11px', fontWeight: 600, color: '#6b7280', marginBottom: '8px', textTransform: 'uppercase', letterSpacing: '0.5px' }}>
          Hızlı Ekle
        </div>
        <div style={{ display: 'flex', flexWrap: 'wrap', gap: '6px' }}>
          {SUGGESTED_INTERESTS.map(s => {
            const added = interests.includes(s)
            return (
              <button
                key={s}
                onClick={() => !added && addSuggested(s)}
                style={{
                  padding: '5px 12px',
                  background: added ? '#e5e7eb' : '#f3f4f6',
                  color: added ? '#9ca3af' : '#374151',
                  border: `1px solid ${added ? '#d1d5db' : '#e5e7eb'}`,
                  borderRadius: '9999px', fontSize: '12px',
                  cursor: added ? 'default' : 'pointer', transition: 'background 0.15s',
                }}
              >
                {added ? '✓ ' : '+ '}{s}
              </button>
            )
          })}
        </div>
      </div>

      {/* ── Öneriler ─────────────────────────────────── */}
      {interests.length > 0 && (
        <>
          <div style={{
            display: 'flex', alignItems: 'center', gap: '12px',
            borderTop: '2px solid #e5e7eb', paddingTop: '24px', marginBottom: '20px',
          }}>
            <h3 style={{ fontSize: '16px', fontWeight: 700, color: '#111827', margin: 0 }}>
              Hobinize Özel Aktüel Ürünler
            </h3>
            {!recLoading && (
              <span style={{ fontSize: '12px', color: '#6b7280' }}>
                {totalProducts} ürün bulundu
              </span>
            )}
          </div>

          {recLoading ? (
            <div style={{ display: 'flex', alignItems: 'center', gap: '10px', padding: '20px 0', color: '#6b7280', fontSize: '13px' }}>
              <Loader size="small" type="converging-spinner" />
              Ürünler aranıyor...
            </div>
          ) : (
            results.map(group => (
              <div key={group.interest} style={{ marginBottom: '28px' }}>
                {/* Grup başlığı */}
                <div style={{ display: 'flex', alignItems: 'center', gap: '10px', marginBottom: '12px' }}>
                  <span style={{
                    padding: '3px 12px', background: '#374151', color: '#fff',
                    borderRadius: '9999px', fontSize: '12px', fontWeight: 700,
                  }}>
                    {group.interest}
                  </span>
                  <span style={{ fontSize: '12px', color: '#9ca3af' }}>
                    {group.products.length === 0 ? 'eşleşen ürün bulunamadı' : `${group.products.length} ürün`}
                  </span>
                </div>

                {group.products.length === 0 ? (
                  <div style={{
                    padding: '14px', background: '#f9fafb',
                    border: '1px dashed #d1d5db', borderRadius: '8px',
                    fontSize: '13px', color: '#9ca3af', textAlign: 'center',
                  }}>
                    Bu ilgi alanına ait aktüel ürün bulunamadı.
                  </div>
                ) : (
                  <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                    {group.products.map(p => {
                      const color = chainColor(p.storeName)
                      const stale = isStale(p.productBringDate)
                      return (
                        <div key={p.productId} style={{
                          background: '#fff', border: '1px solid #e5e7eb',
                          borderLeft: `4px solid ${color}`, borderRadius: '10px',
                          padding: '14px 16px', boxShadow: '0 1px 3px rgba(0,0,0,0.05)',
                        }}>
                          <div style={{ fontWeight: 600, fontSize: '14px', color: '#111827', marginBottom: '3px' }}>
                            {p.productName}
                          </div>
                          <div style={{ fontSize: '12px', color: '#9ca3af', marginBottom: '8px' }}>
                            Kategori: {p.category}
                          </div>
                          <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', flexWrap: 'wrap', gap: '6px' }}>
                            <span style={{
                              background: color, color: '#fff',
                              padding: '2px 10px', borderRadius: '9999px',
                              fontSize: '11px', fontWeight: 700,
                            }}>
                              {p.storeName}
                            </span>
                            <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
                              {p.productBringDate && (
                                <span style={{ fontSize: '11px', color: '#9ca3af' }}>
                                  Geliş: {new Date(p.productBringDate).toLocaleDateString('tr-TR')}
                                </span>
                              )}
                              {stale && (
                                <span style={{
                                  fontSize: '11px', color: '#92400e',
                                  background: '#fffbeb', border: '1px solid #f59e0b',
                                  borderRadius: '6px', padding: '1px 8px',
                                }}>
                                  ⚠ Tükenmiş olabilir
                                </span>
                              )}
                            </div>
                          </div>
                          {/* Eşleşme mesajı */}
                          <div style={{
                            marginTop: '10px', padding: '7px 12px',
                            background: '#f0fdf4', border: '1px solid #bbf7d0',
                            borderRadius: '8px', fontSize: '12px', color: '#166534',
                          }}>
                            <strong>{group.interest}</strong> hobinize ait bu aktüel ürün <strong>{p.storeName}</strong> marketinde bulunmaktadır.
                          </div>
                        </div>
                      )
                    })}
                  </div>
                )}
              </div>
            ))
          )}
        </>
      )}

      {interests.length === 0 && (
        <div style={{
          textAlign: 'center', padding: '40px 20px',
          color: '#9ca3af', fontSize: '14px',
          border: '2px dashed #e5e7eb', borderRadius: '12px',
        }}>
          Yukarıdan ilgi alanı ekleyin, eşleşen aktüel ürünler burada görünecek.
        </div>
      )}
    </div>
  )
}
