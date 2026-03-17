const OVERPASS_URL = 'https://overpass-api.de/api/interpreter'

// Ankara bounding box: south, west, north, east
const BBOX = '39.75,32.55,40.15,33.15'

export async function fetchOsmStores() {
  const query = `
    [out:json][timeout:30];
    (
      node["shop"="supermarket"](${BBOX});
      node["shop"="convenience"]["name"~"Migros|A101|BİM|ŞOK|Şok|CarrefourSA",i](${BBOX});
    );
    out body;
  `

  const res = await fetch(OVERPASS_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
    body: `data=${encodeURIComponent(query)}`
  })

  if (!res.ok) throw new Error('Overpass API hatası')

  const data = await res.json()

  return data.elements.map(el => ({
    id: el.id,
    name: el.tags?.name || el.tags?.operator || 'Market',
    chain: detectChain(el.tags?.name, el.tags?.operator, el.tags?.brand),
    lat: el.lat,
    lon: el.lon,
  }))
}

function detectChain(name = '', operator = '', brand = '') {
  const text = `${name} ${operator} ${brand}`.toLowerCase()
  if (text.includes('migros'))      return 'Migros'
  if (text.includes('a101'))        return 'A101'
  if (text.includes('bim') || text.includes('bİm')) return 'BİM'
  if (text.includes('şok') || text.includes('sok')) return 'Şok'
  if (text.includes('carrefour'))   return 'CarrefourSA'
  if (text.includes('hakmar'))      return 'Hakmar'
  if (text.includes('metro'))       return 'Metro'
  return ''
}
