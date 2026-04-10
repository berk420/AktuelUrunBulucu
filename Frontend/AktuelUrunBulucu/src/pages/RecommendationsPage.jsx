import { useState, useEffect } from 'react'
import { Loader } from '@progress/kendo-react-indicators'
import { getRecommendations } from '../api/recommendationApi'

const STORAGE_KEY = 'aktuel_interests'

const STALE_THRESHOLD_DAYS = 7

const CHAIN_COLORS = {
  Migros:      '#f97316',
  A101:        '#ef4444',
  'BİM':       '#3b82f6',
  'Şok':       '#8b5cf6',
  CarrefourSA: '#10b981',
}

function chainColor(storeName) {
  return CHAIN_COLORS[storeName] || '#6b7280'
}

function isStale(productBringDate) {
  if (!productBringDate) return false
  const diffMs = Date.now() - new Date(productBringDate).getTime()
  return diffMs > STALE_THRESHOLD_DAYS * 24 * 60 * 60 * 1000
}

export default function RecommendationsPage({ onNavigate }) {
  const [results, setResults] = useState([])
  const [interests, setInterests] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)

  useEffect(() => {
    let saved = []
    try {
      saved = JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]')
    } catch {
      saved = []
    }
    setInterests(saved)

    if (saved.length === 0) {
      setLoading(false)
      return
    }

    getRecommendations(saved)
      .then(data => setResults(data.results || []))
      .catch(() => setError('Öneriler yüklenirken bir hata oluştu.'))
      .finally(() => setLoading(false))
  }, [])

  if (loading) {
    return (
      <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: '60vh', gap: '12px' }}>
        <Loader size="large" type="converging-spinner" />
        <span style={{ fontSize: '14px', color: '#6b7280' }}>İlgi alanlarınıza ait ürünler aranıyor...</span>
      </div>
    )
  }

  if (interests.length === 0) {
    return (
      <div style={{ maxWidth: '600px', margin: '0 auto', padding: '24px 16px', textAlign: 'center' }}>
        <p style={{ color: '#6b7280', marginBottom: '16px' }}>Henüz ilgi alanı eklemediniz.</p>
        <button
          onClick={() => onNavigate('interests')}
          style={btnStyle('#374151')}
        >
          İlgi Alanı Ekle
        </button>
      </div>
    )
  }

  if (error) {
    return (
      <div style={{ maxWidth: '600px', margin: '0 auto', padding: '24px 16px', textAlign: 'center' }}>
        <p style={{ color: '#ef4444', marginBottom: '16px' }}>{error}</p>
        <button onClick={() => window.location.reload()} style={btnStyle('#374151')}>Tekrar Dene</button>
      </div>
    )
  }

  const totalProducts = results.reduce((sum, r) => sum + r.products.length, 0)

  return (
    <div style={{ maxWidth: '700px', margin: '0 auto', padding: '24px 16px' }}>
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: '6px', flexWrap: 'wrap', gap: '8px' }}>
        <h2 style={{ fontSize: '20px', fontWeight: 700, color: '#111827' }}>
          Hobinize Özel Aktüel Ürünler
        </h2>
        <button
          onClick={() => onNavigate('interests')}
          style={{
            padding: '6px 14px',
            background: '#f3f4f6',
            color: '#374151',
            border: '1px solid #e5e7eb',
            borderRadius: '8px',
            fontSize: '13px',
            cursor: 'pointer',
          }}
        >
          İlgi Alanlarını Düzenle
        </button>
      </div>
      <p style={{ fontSize: '13px', color: '#6b7280', marginBottom: '24px' }}>
        {interests.length} ilgi alanı — {totalProducts} ürün bulundu
      </p>

      {results.map(group => (
        <div key={group.interest} style={{ marginBottom: '32px' }}>
          {/* Grup başlığı */}
          <div style={{
            display: 'flex',
            alignItems: 'center',
            gap: '10px',
            marginBottom: '14px',
          }}>
            <span style={{
              padding: '4px 14px',
              background: '#374151',
              color: '#fff',
              borderRadius: '9999px',
              fontSize: '13px',
              fontWeight: 700,
            }}>
              {group.interest}
            </span>
            <span style={{ fontSize: '13px', color: '#6b7280' }}>
              {group.products.length === 0
                ? 'eşleşen ürün bulunamadı'
                : `${group.products.length} ürün`}
            </span>
          </div>

          {group.products.length === 0 ? (
            <div style={{
              padding: '16px',
              background: '#f9fafb',
              border: '1px dashed #d1d5db',
              borderRadius: '10px',
              fontSize: '13px',
              color: '#9ca3af',
              textAlign: 'center',
            }}>
              Bu ilgi alanına ait aktüel ürün bulunamadı.
            </div>
          ) : (
            <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
              {group.products.map(p => {
                const color = chainColor(p.storeName)
                const stale = isStale(p.productBringDate)
                return (
                  <div
                    key={p.productId}
                    style={{
                      background: '#fff',
                      border: '1px solid #e5e7eb',
                      borderLeft: `4px solid ${color}`,
                      borderRadius: '10px',
                      padding: '14px 16px',
                      boxShadow: '0 1px 3px rgba(0,0,0,0.06)',
                    }}
                  >
                    {/* Ürün adı */}
                    <div style={{ fontWeight: 600, fontSize: '14px', color: '#111827', marginBottom: '4px' }}>
                      {p.productName}
                    </div>

                    {/* Kategori */}
                    <div style={{ fontSize: '12px', color: '#6b7280', marginBottom: '8px' }}>
                      Kategori: {p.category}
                    </div>

                    {/* Alt bilgi satırı */}
                    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', flexWrap: 'wrap', gap: '6px' }}>
                      <span style={{
                        background: color,
                        color: '#fff',
                        padding: '2px 10px',
                        borderRadius: '9999px',
                        fontSize: '11px',
                        fontWeight: 700,
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
                            fontSize: '11px',
                            color: '#92400e',
                            background: '#fffbeb',
                            border: '1px solid #f59e0b',
                            borderRadius: '6px',
                            padding: '1px 8px',
                          }}>
                            ⚠ Tükenmiş olabilir
                          </span>
                        )}
                      </div>
                    </div>

                    {/* Hobi eşleşme mesajı */}
                    <div style={{
                      marginTop: '10px',
                      padding: '8px 12px',
                      background: '#f0fdf4',
                      border: '1px solid #bbf7d0',
                      borderRadius: '8px',
                      fontSize: '12px',
                      color: '#166534',
                    }}>
                      <strong>{group.interest}</strong> hobinize ait bu aktüel ürün <strong>{p.storeName}</strong> marketinde bulunmaktadır.
                    </div>
                  </div>
                )
              })}
            </div>
          )}
        </div>
      ))}
    </div>
  )
}

function btnStyle(bg) {
  return {
    padding: '10px 24px',
    background: bg,
    color: '#fff',
    border: 'none',
    borderRadius: '8px',
    fontSize: '14px',
    fontWeight: 600,
    cursor: 'pointer',
  }
}
