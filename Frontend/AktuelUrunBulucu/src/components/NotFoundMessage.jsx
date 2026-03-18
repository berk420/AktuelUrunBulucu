import { useState } from 'react'
import { Notification, NotificationGroup } from '@progress/kendo-react-notification'
import { Input } from '@progress/kendo-react-inputs'
import { Button } from '@progress/kendo-react-buttons'

export default function NotFoundMessage({ visible, searchedProduct, onClose, onSubscribe }) {
  const [email, setEmail] = useState('')
  const [submitted, setSubmitted] = useState(false)
  const [submitting, setSubmitting] = useState(false)

  if (!visible) return null

  async function handleSubscribe() {
    if (!email.trim()) return
    setSubmitting(true)
    await onSubscribe(email.trim())
    setSubmitted(true)
    setSubmitting(false)
  }

  return (
    <NotificationGroup style={{ position: 'relative', zIndex: 1, marginTop: '8px' }}>
      <Notification
        type={{ style: 'warning', icon: true }}
        closable
        onClose={() => { onClose(); setSubmitted(false); setEmail('') }}
      >
        {submitted ? (
          <span>
            Bildirim talebiniz alındı! <strong>{searchedProduct}</strong> stoklara girdiğinde mail gönderilecektir.
          </span>
        ) : (
          <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
            <span>
              <strong>"{searchedProduct}"</strong> zincir marketlerde bulunmamaktadır.
              E-posta adresinizi bırakın, geldiğinde haber verelim.
            </span>
            <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
              <Input
                type="email"
                placeholder="ornek@mail.com"
                value={email}
                onChange={e => setEmail(e.value)}
                style={{ flex: 1 }}
              />
              <Button
                themeColor="primary"
                onClick={handleSubscribe}
                disabled={!email.trim() || submitting}
              >
                {submitting ? 'Gönderiliyor...' : 'Bildir'}
              </Button>
            </div>
          </div>
        )}
      </Notification>
    </NotificationGroup>
  )
}
