import { Card, CardTitle, CardBody } from '@progress/kendo-react-layout'

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

export default function StoreList({ products }) {
  if (!products || products.length === 0) return null

  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
      <h3 style={{ margin: 0, fontSize: '16px', fontWeight: 600, color: '#374151' }}>
        {products.length} sonuç bulundu
      </h3>
      {products.map(p => (
        <Card key={p.productId} style={{ borderLeft: `4px solid ${chainColor(p.storeName)}` }}>
          <CardBody>
            <CardTitle style={{ fontSize: '14px', fontWeight: 600, marginBottom: '6px' }}>
              {p.productName}
            </CardTitle>
            <div style={{ fontSize: '12px', color: '#6b7280', marginBottom: '4px' }}>
              Kategori: {p.category}
            </div>
            {p.productBringDate && (
              <div style={{ fontSize: '12px', color: '#6b7280', marginBottom: '4px' }}>
                Geliş Tarihi: {new Date(p.productBringDate).toLocaleDateString('tr-TR')}
              </div>
            )}
            <div style={{ marginTop: '8px' }}>
              <span style={{
                background: chainColor(p.storeName),
                color: '#fff',
                padding: '2px 10px',
                borderRadius: '9999px',
                fontSize: '11px',
                fontWeight: 600
              }}>
                {p.storeName}
              </span>
            </div>
          </CardBody>
        </Card>
      ))}
    </div>
  )
}
