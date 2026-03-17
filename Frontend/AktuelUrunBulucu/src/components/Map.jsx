import { MapContainer, TileLayer, Marker, Popup, Circle, useMap } from 'react-leaflet'
import { useEffect } from 'react'
import L from 'leaflet'

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

// OSM store'un chain'i, arama sonucundaki storeName ile eşleşiyor mu?
function matchesStoreName(osmChain, osmName, storeName) {
  if (!storeName) return false
  const needle = storeName.toLowerCase()
  return (
    (osmChain && osmChain.toLowerCase().includes(needle)) ||
    (osmName && osmName.toLowerCase().includes(needle))
  )
}

export default function Map({ osmStores, matchedStoreNames, userCoords }) {
  const highlighted = new Set(matchedStoreNames.map(n => n.toLowerCase()))

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

      {/* Kullanıcı konumu + 10km çember */}
      {userCoords && (
        <>
          <Circle
            center={[userCoords.latitude, userCoords.longitude]}
            radius={10000}
            pathOptions={{ color: '#3b82f6', fillColor: '#3b82f6', fillOpacity: 0.05, weight: 1.5 }}
          />
          <Marker position={[userCoords.latitude, userCoords.longitude]} icon={USER_ICON} zIndexOffset={2000}>
            <Popup>Konumunuz</Popup>
          </Marker>
        </>
      )}

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
