const API_URL = import.meta.env.VITE_API_URL

export async function getRecommendations(interests) {
  if (!interests || interests.length === 0) return { results: [] }
  const params = interests.map(i => `interests=${encodeURIComponent(i)}`).join('&')
  const res = await fetch(`${API_URL}/api/recommendations?${params}`)
  if (!res.ok) throw new Error('API hatası')
  return res.json()
}
