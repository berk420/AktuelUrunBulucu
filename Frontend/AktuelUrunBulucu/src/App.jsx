import { useState, useEffect, useRef, useCallback } from 'react'
import './App.css'
import { Loader } from '@progress/kendo-react-indicators'
import SearchBar from './components/SearchBar'
import Map from './components/Map'
import StoreList from './components/StoreList'
import NotFoundMessage from './components/NotFoundMessage'
import InterestsPage from './pages/InterestsPage'
import DiscountedProductsPage from './pages/DiscountedProductsPage'
import { searchProducts, getUserIp, saveUserLocation, subscribeNotification } from './api/searchApi'
import { fetchOsmStores } from './api/overpassApi'
import { haversineKm } from './utils/distance'

const MAX_RADIUS_KM = 10
const RATE_LIMIT = 10
const RATE_LIMIT_WINDOW_MS = 60_000

const MOBILE_BREAKPOINT = 768
const PANEL_WIDTH = 300
const SWIPE_CLOSE_THRESHOLD = 0.4

const NAV_ITEMS = [
  { key: 'home',       label: 'Ürün Ara' },
  { key: 'interests',  label: 'İlgi Alanlarım' },
  { key: 'discounted', label: 'Öneriler' },
]

export default function App() {
  const [page, setPage] = useState('home')
  const [query, setQuery] = useState('')
  const [osmStores, setOsmStores] = useState([])
  const [userCoords, setUserCoords] = useState(null)
  const [matchedProducts, setMatchedProducts] = useState([])
  const [notFound, setNotFound] = useState(false)
  const [lastQuery, setLastQuery] = useState('')
  const [loading, setLoading] = useState(false)
  const [osmLoading, setOsmLoading] = useState(true)
  const [searchPerformed, setSearchPerformed] = useState(false)
  const [rateLimitWarning, setRateLimitWarning] = useState(false)
  const [searchError, setSearchError] = useState(null)
  const [isMobile, setIsMobile] = useState(window.innerWidth < MOBILE_BREAKPOINT)
  const [panelOpen, setPanelOpen] = useState(true)
  const [dragX, setDragX] = useState(0)
  const [isDragging, setIsDragging] = useState(false)
  const [selectedProduct, setSelectedProduct] = useState(null)
  const searchCountRef = useRef(0)
  const windowStartRef = useRef(Date.now())
  const dragStartX = useRef(0)

  useEffect(() => {
    const handleResize = () => setIsMobile(window.innerWidth < MOBILE_BREAKPOINT)
    window.addEventListener('resize', handleResize)
    return () => window.removeEventListener('resize', handleResize)
  }, [])

  useEffect(() => {
    if (matchedProducts.length > 0) {
      setPanelOpen(true)
      setDragX(0)
      setSelectedProduct(null)
    }
  }, [matchedProducts])

  const handleTouchStart = useCallback((e) => {
    dragStartX.current = e.touches[0].clientX
    setIsDragging(true)
  }, [])

  const handleTouchMove = useCallback((e) => {
    const delta = e.touches[0].clientX - dragStartX.current
    if (delta > 0) setDragX(delta)
  }, [])

  const handleTouchEnd = useCallback(() => {
    setIsDragging(false)
    if (dragX > PANEL_WIDTH * SWIPE_CLOSE_THRESHOLD) setPanelOpen(false)
    setDragX(0)
  }, [dragX])

  useEffect(() => {
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
    fetchOsmStores()
      .then(stores => setOsmStores(stores))
      .catch(err => console.error('OSM yüklenemedi:', err))
      .finally(() => setOsmLoading(false))
  }, [])

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
    setRateLimitWarning(false)
    setSearchError(null)
    setSearchPerformed(true)
    try {
      const ip = await getUserIp()
      const result = await searchProducts(query.trim(), ip)
      if (result.found) {
        setMatchedProducts(result.matchedProducts)
      } else {
        setNotFound(true)
        setLastQuery(query.trim())
      }
    } catch (err) {
      console.error(err)
      setSearchError({ title: err.title || 'Bir sorun çıktı', detail: err.message || 'Bilinmeyen hata' })
    } finally {
      setLoading(false)
    }
  }

  const matchedStoreNames = [...new Set(matchedProducts.map(p => p.storeName))]

  return (
    <div style={{ display: 'flex', flexDirection: 'column', height: '100vh' }}>
      {/* ── HEADER ────────────────────────────────────────────── */}
      <div style={{
        background: '#fff',
        borderBottom: '1px solid #e5e7eb',
        boxShadow: '0 1px 3px rgba(0,0,0,0.08)',
      }}>
        {/* Başlık */}
        <div style={{ padding: '14px 24px 0' }}>
          <h1 style={{ fontSize: '22px', fontWeight: 800, color: '#111827', margin: 0 }}>
            Aktüel Bulucu
          </h1>
          <p style={{ fontSize: '12px', color: '#9ca3af', margin: '2px 0 0' }}>
            Marketlerdeki kampanyalı aktüel ürünleri keşfet
          </p>
        </div>

        {/* Sekme navigasyonu */}
        <div style={{
          display: 'flex',
          gap: '0',
          marginTop: '12px',
          borderTop: '1px solid #f3f4f6',
        }}>
          {NAV_ITEMS.map(item => (
            <button
              key={item.key}
              onClick={() => setPage(item.key)}
              style={{
                flex: 1,
                padding: '12px 8px',
                background: 'none',
                border: 'none',
                borderBottom: page === item.key ? '3px solid #374151' : '3px solid transparent',
                color: page === item.key ? '#111827' : '#6b7280',
                fontSize: '13px',
                fontWeight: page === item.key ? 700 : 400,
                cursor: 'pointer',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                gap: '3px',
                transition: 'color 0.15s, border-color 0.15s',
              }}
            >
              <span>{item.label}</span>
            </button>
          ))}
        </div>

        {/* Ürün Ara sekmesi içeriği (arama kutusu + uyarılar) */}
        {page === 'home' && (
          <div style={{ padding: '12px 24px 16px' }}>
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
            {rateLimitWarning && (
              <div style={{
                display: 'flex', alignItems: 'center', justifyContent: 'space-between',
                gap: '8px', marginTop: '8px', padding: '10px 14px',
                background: '#fef2f2', border: '1px solid #ef4444',
                borderRadius: '8px', fontSize: '13px', color: '#991b1b',
              }}>
                <span>🛑 Çok fazla arama yaptınız. Biraz dinlenin, 1 dakika sonra tekrar deneyin.</span>
                <button onClick={() => setRateLimitWarning(false)}
                  style={{ background: 'none', border: 'none', cursor: 'pointer', fontSize: '16px', color: '#991b1b', lineHeight: 1 }}>✕</button>
              </div>
            )}
            {searchError && (
              <div style={{
                display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between',
                gap: '8px', marginTop: '8px', padding: '10px 14px',
                background: '#fef2f2', border: '1px solid #ef4444',
                borderRadius: '8px', fontSize: '13px', color: '#991b1b',
              }}>
                <div>
                  <strong>{searchError.title}</strong>
                  <div style={{ marginTop: '2px', opacity: 0.85 }}>{searchError.detail}</div>
                </div>
                <button onClick={() => setSearchError(null)}
                  style={{ background: 'none', border: 'none', cursor: 'pointer', fontSize: '16px', color: '#991b1b', lineHeight: 1, flexShrink: 0 }}>✕</button>
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
        )}
      </div>

      {/* ── BODY ──────────────────────────────────────────────── */}

      {page === 'interests' && (
        <div style={{ flex: 1, overflowY: 'auto', background: '#f9fafb' }}>
          <InterestsPage />
        </div>
      )}

      {page === 'discounted' && (
        <div style={{ flex: 1, overflowY: 'auto', background: '#f9fafb' }}>
          <DiscountedProductsPage />
        </div>
      )}

      {page === 'home' && (
        <div style={{ display: 'flex', flex: 1, overflow: 'hidden', position: 'relative' }}>
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
                selectedProduct={selectedProduct}
              />
            )}
          </div>

          {matchedProducts.length > 0 && !isMobile && (
            <div style={{ width: '340px', overflowY: 'auto', padding: '16px', background: '#f9fafb', borderLeft: '1px solid #e5e7eb' }}>
              <StoreList
                products={matchedProducts}
                selectedProduct={selectedProduct}
                onProductClick={setSelectedProduct}
              />
            </div>
          )}

          {/* Mobil: sürüklenebilir panel */}
          {matchedProducts.length > 0 && isMobile && (
            <>
              <div style={{
                position: 'absolute', top: 0, right: 0, bottom: 0,
                width: `${PANEL_WIDTH}px`, background: '#f9fafb',
                borderLeft: '1px solid #e5e7eb', display: 'flex',
                flexDirection: 'column', overflow: 'hidden', zIndex: 500,
                transform: panelOpen ? `translateX(${dragX}px)` : `translateX(${PANEL_WIDTH}px)`,
                transition: isDragging ? 'none' : 'transform 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                boxShadow: '-4px 0 16px rgba(0,0,0,0.12)',
              }}>
                <div onTouchStart={handleTouchStart} onTouchMove={handleTouchMove} onTouchEnd={handleTouchEnd}
                  style={{
                    position: 'absolute', left: '-48px', top: '50%',
                    transform: 'translateY(-50%)', background: '#374151', color: '#fff',
                    width: '48px', padding: '24px 0', borderRadius: '12px 0 0 12px',
                    display: 'flex', flexDirection: 'column', alignItems: 'center',
                    gap: '6px', cursor: 'grab', touchAction: 'none', userSelect: 'none',
                    boxShadow: '-2px 0 8px rgba(0,0,0,0.2)',
                  }}>
                  <span style={{ fontSize: '20px', lineHeight: 1 }}>›</span>
                  <span style={{ fontSize: '9px', writingMode: 'vertical-rl', letterSpacing: '0.5px', opacity: 0.8 }}>KAPAT</span>
                </div>
                <div style={{
                  display: 'flex', alignItems: 'center', justifyContent: 'space-between',
                  padding: '12px 16px', borderBottom: '1px solid #e5e7eb',
                  background: '#fff', flexShrink: 0,
                }}>
                  <span style={{ fontSize: '14px', fontWeight: 600, color: '#374151' }}>
                    {matchedProducts.length} sonuç bulundu
                  </span>
                  <button onClick={() => setPanelOpen(false)} style={{
                    background: '#374151', color: '#fff', border: 'none',
                    borderRadius: '50%', width: '28px', height: '28px',
                    fontSize: '14px', cursor: 'pointer', display: 'flex',
                    alignItems: 'center', justifyContent: 'center', lineHeight: 1, flexShrink: 0,
                  }}>✕</button>
                </div>
                <div style={{ flex: 1, overflowY: 'auto', padding: '16px' }}>
                  <StoreList
                    products={matchedProducts}
                    selectedProduct={selectedProduct}
                    onProductClick={setSelectedProduct}
                  />
                </div>
              </div>
              {!panelOpen && (
                <button onClick={() => setPanelOpen(true)} style={{
                  position: 'absolute', right: 0, top: '50%',
                  transform: 'translateY(-50%)', background: '#374151',
                  color: '#fff', border: 'none', padding: '16px 10px',
                  borderRadius: '10px 0 0 10px', cursor: 'pointer', zIndex: 501,
                  display: 'flex', flexDirection: 'column', alignItems: 'center',
                  gap: '4px', boxShadow: '-2px 0 8px rgba(0,0,0,0.2)',
                }}>
                  <span style={{ fontSize: '18px', lineHeight: 1 }}>‹</span>
                  <span style={{ fontSize: '9px', writingMode: 'vertical-rl', letterSpacing: '0.5px', opacity: 0.8 }}>SONUÇLAR</span>
                </button>
              )}
            </>
          )}
        </div>
      )}
    </div>
  )
}
