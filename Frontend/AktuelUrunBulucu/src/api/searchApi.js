const API_URL = import.meta.env.VITE_API_URL

export async function getUserIp() {
  const res = await fetch('https://api.ipify.org?format=json')
  const data = await res.json()
  return data.ip
}

export async function searchProducts(query, ip) {
  const res = await fetch(`${API_URL}/api/search?query=${encodeURIComponent(query)}&ip=${encodeURIComponent(ip)}`)
  if (!res.ok) {
    let title = 'Bir sorun çıktı'
    let detail = 'Bilinmeyen hata'
    try {
      const body = await res.json()
      if (body.title) title = body.title
      if (body.detail) detail = body.detail
    } catch {
      // JSON parse edilemezse default mesajları kullan
    }
    const err = new Error(detail)
    err.title = title
    throw err
  }
  return res.json()
}

export async function saveUserLocation(ip, latitude, longitude) {
  await fetch(`${API_URL}/api/location`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ ip, latitude, longitude })
  })
}

export async function subscribeNotification(ip, email, product) {
  const res = await fetch(`${API_URL}/api/notify`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ ip, email, product })
  })
  if (!res.ok) throw new Error('Bildirim kaydedilemedi')
}
