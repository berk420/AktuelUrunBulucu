import { Notification, NotificationGroup } from '@progress/kendo-react-notification'

export default function NotFoundMessage({ visible, onClose }) {
  if (!visible) return null

  return (
    <NotificationGroup style={{ position: 'relative', zIndex: 1 }}>
      <Notification
        type={{ style: 'warning', icon: true }}
        closable
        onClose={onClose}
      >
        <span>
          Aradığınız ürün zincir marketlerde bulunmamaktadır, geldiği taktirde size mail atılacaktır.
        </span>
      </Notification>
    </NotificationGroup>
  )
}
