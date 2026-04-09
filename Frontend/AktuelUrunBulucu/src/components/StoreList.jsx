import { Card, CardBody, CardTitle } from '@progress/kendo-react-layout'

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

export default function StoreList({ products, selectedProduct, onProductClick }) {
  if (!products || products.length === 0) return null

  return (
    <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
      {products.map(p => {
        const isSelected = selectedProduct?.productId === p.productId
        const color = chainColor(p.storeName)
        return (
          <Card
            key={p.productId}
            onClick={() => onProductClick(isSelected ? null : p)}
            style={{
              borderLeft: `4px solid ${color}`,
              cursor: 'pointer',
              outline: isSelected ? `2px solid ${color}` : '2px solid transparent',
              outlineOffset: '2px',
              transition: 'outline 0.15s, box-shadow 0.15s',
              boxShadow: isSelected ? `0 0 0 3px ${color}22` : undefined,
            }}
          >
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
              <div style={{ marginTop: '8px', display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                <span style={{
                  background: color,
                  color: '#fff',
                  padding: '2px 10px',
                  borderRadius: '9999px',
                  fontSize: '11px',
                  fontWeight: 600,
                }}>
                  {p.storeName}
                </span>
                {isSelected && (
                  <span style={{ fontSize: '11px', color: color, fontWeight: 600 }}>
                    📍 Haritada gösteriliyor
                  </span>
                )}
              </div>
            </CardBody>
          </Card>
        )
      })}
    </div>
  )
}
