import { useState, useEffect } from 'react'
import { Loader } from '@progress/kendo-react-indicators'
import { getDiscountedProducts } from '../api/recommendationApi'

const STALE_THRESHOLD_DAYS = 7

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

export default function DiscountedProductsPage() {
  const [groups, setGroups] = useState([])   // [{ store, products[] }]
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(null)
  const [activeStore, setActiveStore] = useState(null)

  useEffect(() => {
    getDiscountedProducts()
      .then(products => {
        /* Markete göre grupla */
        const map = {}
        for (const p of products) {
          if (!map[p.storeName]) map[p.storeName] = []
          map[p.storeName].push(p)
        }
        const grouped = Object.entries(map).map(([store, items]) => ({ store, products: items }))
        grouped.sort((a, b) => b.products.length - a.products.length)
        setGroups(grouped)
        if (grouped.length > 0) setActiveStore(grouped[0].store)
      })
      .catch(() => setError('Ürünler yüklenirken bir hata oluştu.'))
      .finally(() => setLoading(false))
  }, [])

  if (loading) {
    return (
      <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: '60vh', gap: '12px' }}>
        <Loader size="large" type="converging-spinner" />
        <span style={{ fontSize: '14px', color: '#6b7280' }}>Kampanyalı ürünler yükleniyor...</span>
      </div>
    )
  }

  if (error) {
    return (
      <div style={{ maxWidth: '600px', margin: '40px auto', padding: '0 16px', textAlign: 'center', color: '#ef4444', fontSize: '14px' }}>
        {error}
      </div>
    )
  }

  const totalProducts = groups.reduce((s, g) => s + g.products.length, 0)
  const activeGroup = groups.find(g => g.store === activeStore)

  return (
    <div style={{ maxWidth: '680px', margin: '0 auto', padding: '24px 16px' }}>

      {/* Başlık */}
      <h2 style={{ fontSize: '18px', fontWeight: 700, color: '#111827', marginBottom: '4px' }}>
        Piyasa Fiyatının Altında Ürünler
      </h2>
      <p style={{ fontSize: '13px', color: '#6b7280', marginBottom: '20px' }}>
        Aktüel ürünler normal piyasa fiyatından daha uygun fiyata sunulmaktadır. ({totalProducts} ürün)
      </p>

      {/* Market sekmeleri */}
      <div style={{
        display: 'flex', gap: '8px', flexWrap: 'wrap', marginBottom: '24px',
        padding: '12px', background: '#f3f4f6', borderRadius: '12px',
      }}>
        {groups.map(g => {
          const color = chainColor(g.store)
          const active = g.store === activeStore
          return (
            <button
              key={g.store}
              onClick={() => setActiveStore(g.store)}
              style={{
                padding: '8px 16px',
                background: active ? color : '#fff',
                color: active ? '#fff' : '#374151',
                border: `2px solid ${active ? color : '#e5e7eb'}`,
                borderRadius: '9999px',
                fontSize: '13px',
                fontWeight: active ? 700 : 400,
                cursor: 'pointer',
                transition: 'all 0.15s',
                display: 'flex',
                alignItems: 'center',
                gap: '6px',
              }}
            >
              {g.store}
              <span style={{
                background: active ? 'rgba(255,255,255,0.3)' : '#f3f4f6',
                padding: '1px 7px',
                borderRadius: '9999px',
                fontSize: '11px',
                fontWeight: 700,
                color: active ? '#fff' : '#6b7280',
              }}>
                {g.products.length}
              </span>
            </button>
          )
        })}
      </div>

      {/* Ürün listesi */}
      {activeGroup && (
        <div>
          <div style={{ fontSize: '13px', fontWeight: 600, color: '#374151', marginBottom: '12px' }}>
            {activeGroup.store} — {activeGroup.products.length} kampanyalı ürün
          </div>
          <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
            {activeGroup.products.map(p => {
              const color = chainColor(p.storeName)
              const stale = isStale(p.productBringDate)
              return (
                <div key={p.productId} style={{
                  background: '#fff',
                  border: '1px solid #e5e7eb',
                  borderLeft: `4px solid ${color}`,
                  borderRadius: '10px',
                  padding: '14px 16px',
                  boxShadow: '0 1px 3px rgba(0,0,0,0.05)',
                }}>
                  {/* İndirim rozeti */}
                  <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', gap: '8px', marginBottom: '4px' }}>
                    <span style={{ fontWeight: 600, fontSize: '14px', color: '#111827' }}>
                      {p.productName}
                    </span>
                    <span style={{
                      flexShrink: 0,
                      padding: '2px 8px',
                      background: '#dcfce7',
                      color: '#166534',
                      border: '1px solid #bbf7d0',
                      borderRadius: '6px',
                      fontSize: '11px',
                      fontWeight: 700,
                      whiteSpace: 'nowrap',
                    }}>
                      Piyasa Altı Fiyat
                    </span>
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
                </div>
              )
            })}
          </div>
        </div>
      )}
    </div>
  )
}
