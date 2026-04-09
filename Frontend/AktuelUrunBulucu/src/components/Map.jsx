import { MapContainer, TileLayer, Marker, Popup, Circle, useMap } from 'react-leaflet'
import { useEffect } from 'react'
import L from 'leaflet'
import { haversineKm } from '../utils/distance'

delete L.Icon.Default.prototype._getIconUrl
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
  iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
})

const SHADOW = 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png'

function makeIcon(color) {
  return new L.Icon({
    iconUrl: `https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-${color}.png`,
    shadowUrl: SHADOW,
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowSize: [41, 41],
  })
}

const GREY_ICON = makeIcon('grey')

const CHAIN_ICONS = {
  Migros:      makeIcon('orange'),
  A101:        makeIcon('red'),
  'BİM':       makeIcon('blue'),
  'Şok':       makeIcon('violet'),
  CarrefourSA: makeIcon('green'),
}

function chainIcon(chain) {
  return CHAIN_ICONS[chain] || makeIcon('yellow')
}

const USER_ICON = new L.Icon({
  iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-gold.png',
  shadowUrl: SHADOW,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41],
})

function RecenterMap({ coords }) {
  const map = useMap()
  useEffect(() => {
    if (coords) map.setView([coords.latitude, coords.longitude], 13)
  }, [coords, map])
  return null
}

/// <summary>
/// Seçili ürünün zinciriyle eşleşen en yakın OSM mağazasına haritayı uçurur.
/// </summary>
function FlyToNearestStore({ osmStores, selectedStoreName, userCoords }) {
  const map = useMap()

  useEffect(() => {
    if (!selectedStoreName || !userCoords) return

    const matching = osmStores.filter(s =>
      matchesStoreName(s.chain, s.name, selectedStoreName)
    )
    if (matching.length === 0) return

    const nearest = matching.reduce((best, s) => {
      const d = haversineKm(userCoords.latitude, userCoords.longitude, s.lat, s.lon)
      return d < best.dist ? { store: s, dist: d } : best
    }, { store: matching[0], dist: Infinity })

    map.flyTo([nearest.store.lat, nearest.store.lon], 16, { duration: 1.2 })
  }, [selectedStoreName, osmStores, userCoords, map])

  return null
}

// OSM store'un chain'i, arama sonucundaki storeName ile eşleşiyor mu?
function matchesStoreName(osmChain, osmName, storeName) {
  if (!storeName) return false
  const needle = storeName.toLowerCase()
  return (
    (osmChain && osmChain.toLowerCase().includes(needle)) ||
    (osmName && osmName.toLowerCase().includes(needle))
  )
}

const DISTANCE_RINGS = [
  { radius: 1000, color: '#22c55e', label: '1 km' },
  { radius: 3000, color: '#f59e0b', label: '3 km' },
  { radius: 5000, color: '#ef4444', label: '5 km' },
]

export default function Map({ osmStores, matchedStoreNames, userCoords, searchPerformed, selectedProduct }) {
  // Ürün seçiliyse sadece o zinciri vurgula, yoksa tüm eşleşenleri göster
  const highlighted = selectedProduct
    ? new Set([selectedProduct.storeName.toLowerCase()])
    : new Set(matchedStoreNames.map(n => n.toLowerCase()))

  return (
    <MapContainer
      center={[39.9208, 32.8541]}
      zoom={12}
      style={{ width: '100%', height: '100%', minHeight: '500px', borderRadius: '8px' }}
    >
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
      />

      {userCoords && <RecenterMap coords={userCoords} />}

      {/* Ürün seçilince en yakın mağazaya uç */}
      {selectedProduct && userCoords && (
        <FlyToNearestStore
          osmStores={osmStores}
          selectedStoreName={selectedProduct.storeName}
          userCoords={userCoords}
        />
      )}

      {/* Kullanıcı konumu marker */}
      {userCoords && (
        <Marker position={[userCoords.latitude, userCoords.longitude]} icon={USER_ICON} zIndexOffset={2000}>
          <Popup>Konumunuz</Popup>
        </Marker>
      )}

      {/* Arama yapılınca 1km / 3km / 5km mesafe halkaları */}
      {userCoords && searchPerformed && DISTANCE_RINGS.map(ring => (
        <Circle
          key={ring.radius}
          center={[userCoords.latitude, userCoords.longitude]}
          radius={ring.radius}
          pathOptions={{
            color: ring.color,
            fillColor: ring.color,
            fillOpacity: 0.04,
            weight: 1.5,
            dashArray: '6 4',
          }}
        >
          <Popup>{ring.label} yarıçap</Popup>
        </Circle>
      ))}

      {osmStores.map(store => {
        const isHighlighted = [...highlighted].some(name =>
          matchesStoreName(store.chain, store.name, name)
        )

        return (
          <Marker
            key={`osm-${store.id}`}
            position={[store.lat, store.lon]}
            icon={isHighlighted ? chainIcon(store.chain) : GREY_ICON}
            zIndexOffset={isHighlighted ? 1000 : 0}
          >
            <Popup>
              <strong>{store.name}</strong>
              {store.chain && (
                <>
                  <br />
                  <span style={{ fontSize: '12px', color: '#6b7280' }}>{store.chain}</span>
                </>
              )}
            </Popup>
          </Marker>
        )
      })}
    </MapContainer>
  )
}
