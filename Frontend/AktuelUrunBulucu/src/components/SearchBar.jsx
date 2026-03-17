import { Input } from '@progress/kendo-react-inputs'
import { Button } from '@progress/kendo-react-buttons'

export default function SearchBar({ query, onQueryChange, onSearch, loading }) {
  function handleKeyDown(e) {
    if (e.key === 'Enter') onSearch()
  }

  return (
    <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
      <Input
        value={query}
        onChange={e => onQueryChange(e.value)}
        onKeyDown={handleKeyDown}
        placeholder="Ürün adı yazın... (örn: bisiklet, çamaşır makinesi)"
        style={{ flex: 1 }}
        disabled={loading}
      />
      <Button
        themeColor="primary"
        onClick={onSearch}
        disabled={loading || !query.trim()}
      >
        {loading ? 'Aranıyor...' : 'Ara'}
      </Button>
    </div>
  )
}
