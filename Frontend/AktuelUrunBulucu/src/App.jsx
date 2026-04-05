import { useState, useEffect, useRef } from 'react'
import './App.css'
import { Loader } from '@progress/kendo-react-indicators'
import SearchBar from './components/SearchBar'
import Map from './components/Map'
import StoreList from './components/StoreList'
import NotFoundMessage from './components/NotFoundMessage'
import { searchProducts, getUserIp, saveUserLocation, subscribeNotification } from './api/searchApi'
import { fetchOsmStores } from './api/overpassApi'
import { haversineKm } from './utils/distance'

const MAX_RADIUS_KM = 10
const STALE_THRESHOLD_DAYS = 7
const RATE_LIMIT = 10
const RATE_LIMIT_WINDOW_MS = 60_000

function isStale(productBringDate) {
  if (!productBringDate) return false
  const diffMs = Date.now() - new Date(productBringDate).getTime()
  return diffMs > STALE_THRESHOLD_DAYS * 24 * 60 * 60 * 1000
}

export default function App() {
  const [query, setQuery] = useState('')
  const [osmStores, setOsmStores] = useState([])
  const [userCoords, setUserCoords] = useState(null)
  const [matchedProducts, setMatchedProducts] = useState([])
  const [notFound, setNotFound] = useState(false)
  const [lastQuery, setLastQuery] = useState('')
  const [loading, setLoading] = useState(false)
  const [osmLoading, setOsmLoading] = useState(true)
  const [searchPerformed, setSearchPerformed] = useState(false)
  const [staleWarning, setStaleWarning] = useState(false)
  const [rateLimitWarning, setRateLimitWarning] = useState(false)
  const searchCountRef = useRef(0)
  const windowStartRef = useRef(Date.now())

  useEffect(() => {
    // Konum al ve kaydet
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(async position => {
        const { latitude, longitude } = position.coords
        setUserCoords({ latitude, longitude })

        try {
          const ip = await getUserIp()
          await saveUserLocation(ip, latitude, longitude)
        } catch (err) {
          console.error('Konum kaydedilemedi:', err)
        }
      })
    }

    // OSM marketleri yükle
    fetchOsmStores()
      .then(stores => setOsmStores(stores))
      .catch(err => console.error('OSM yüklenemedi:', err))
      .finally(() => setOsmLoading(false))
  }, [])

  // Kullanıcı konumuna göre 10km içindeki OSM marketleri
  const nearbyOsmStores = userCoords
    ? osmStores.filter(s =>
        haversineKm(userCoords.latitude, userCoords.longitude, s.lat, s.lon) <= MAX_RADIUS_KM
      )
    : osmStores

  async function handleSearch() {
    if (!query.trim()) return

    const now = Date.now()
    if (now - windowStartRef.current > RATE_LIMIT_WINDOW_MS) {
      windowStartRef.current = now
      searchCountRef.current = 0
    }
    searchCountRef.current += 1
    if (searchCountRef.current > RATE_LIMIT) {
      setRateLimitWarning(true)
      return
    }

    setLoading(true)
    setNotFound(false)
    setMatchedProducts([])
    setStaleWarning(false)
    setRateLimitWarning(false)
    setSearchPerformed(true)

    try {
      const ip = await getUserIp()
      const result = await searchProducts(query.trim(), ip)

      if (result.found) {
        setMatchedProducts(result.matchedProducts)
        setStaleWarning(result.matchedProducts.some(p => isStale(p.productBringDate)))
      } else {
        setNotFound(true)
        setLastQuery(query.trim())
      }
    } catch (err) {
      console.error(err)
    } finally {
      setLoading(false)
    }
  }

  const matchedStoreNames = [...new Set(matchedProducts.map(p => p.storeName))]

  return (
    <div style={{ display: 'flex', flexDirection: 'column', height: '100vh' }}>
      {/* Header */}
      <div style={{
        background: '#fff',
        borderBottom: '1px solid #e5e7eb',
        padding: '16px 24px',
        boxShadow: '0 1px 3px rgba(0,0,0,0.1)'
      }}>
        <h1 style={{ fontSize: '20px', fontWeight: 700, color: '#111827', marginBottom: '12px' }}>
          Aktüel Ürün Bulucu
        </h1>
        {userCoords && (
          <div style={{ fontSize: '12px', color: '#6b7280', marginBottom: '8px' }}>
            Konumunuz alındı — {MAX_RADIUS_KM} km çapındaki marketler gösteriliyor
          </div>
        )}
        <SearchBar
          query={query}
          onQueryChange={setQuery}
          onSearch={handleSearch}
          loading={loading}
        />
        {loading && (
          <div style={{ marginTop: '8px', display: 'flex', alignItems: 'center', gap: '8px' }}>
            <Loader size="small" type="converging-spinner" />
            <span style={{ fontSize: '13px', color: '#6b7280' }}>Ürün aranıyor...</span>
          </div>
        )}
        {staleWarning && (
          <div style={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
            gap: '8px',
            marginTop: '8px',
            padding: '10px 14px',
            background: '#fffbeb',
            border: '1px solid #f59e0b',
            borderRadius: '8px',
            fontSize: '13px',
            color: '#92400e',
          }}>
            <span>
              ⚠️ Bu ürünün stoklara giriş tarihi <strong>{STALE_THRESHOLD_DAYS} günden</strong> fazla geçmiş — ürün tükenmiş olabilir.
            </span>
            <button
              onClick={() => setStaleWarning(false)}
              style={{ background: 'none', border: 'none', cursor: 'pointer', fontSize: '16px', color: '#92400e', lineHeight: 1 }}
            >✕</button>
          </div>
        )}

        {rateLimitWarning && (
          <div style={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
            gap: '8px',
            marginTop: '8px',
            padding: '10px 14px',
            background: '#fef2f2',
            border: '1px solid #ef4444',
            borderRadius: '8px',
            fontSize: '13px',
            color: '#991b1b',
          }}>
            <span>🛑 Çok fazla arama yaptınız. Biraz dinlenin, 1 dakika sonra tekrar deneyin.</span>
            <button
              onClick={() => setRateLimitWarning(false)}
              style={{ background: 'none', border: 'none', cursor: 'pointer', fontSize: '16px', color: '#991b1b', lineHeight: 1 }}
            >✕</button>
          </div>
        )}

        <NotFoundMessage
          visible={notFound}
          searchedProduct={lastQuery}
          onClose={() => setNotFound(false)}
          onSubscribe={async (email) => {
            const ip = await getUserIp()
            await subscribeNotification(ip, email, lastQuery)
          }}
        />
      </div>

      {/* Body */}
      <div style={{ display: 'flex', flex: 1, overflow: 'hidden' }}>
        <div style={{ flex: 1, position: 'relative' }}>
          {osmLoading ? (
            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%', flexDirection: 'column', gap: '12px' }}>
              <Loader size="large" type="converging-spinner" />
              <span style={{ fontSize: '13px', color: '#6b7280' }}>OpenStreetMap'den marketler yükleniyor...</span>
            </div>
          ) : (
            <Map
              osmStores={nearbyOsmStores}
              matchedStoreNames={matchedStoreNames}
              userCoords={userCoords}
              searchPerformed={searchPerformed}
            />
          )}
        </div>

        {matchedProducts.length > 0 && (
          <div style={{
            width: '340px',
            overflowY: 'auto',
            padding: '16px',
            background: '#f9fafb',
            borderLeft: '1px solid #e5e7eb'
          }}>
            <StoreList products={matchedProducts} />
          </div>
        )}
      </div>
    </div>
  )
}
