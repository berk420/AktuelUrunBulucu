#!/usr/bin/env bash
# Backend container'ını önceki image versiyonuna geri döndürür.
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT_DIR"

IMAGE_NAME="aktuel-backend"
HEALTH_URL="http://localhost:5012/health"

echo "=== Rollback Başladı ==="

if ! docker image inspect "${IMAGE_NAME}:previous" >/dev/null 2>&1; then
    echo "Önceki versiyon bulunamadı: ${IMAGE_NAME}:previous" >&2
    exit 1
fi

echo "Önceki image geri yükleniyor: ${IMAGE_NAME}:previous -> ${IMAGE_NAME}:latest"
docker tag "${IMAGE_NAME}:previous" "${IMAGE_NAME}:latest"

echo "Container yeniden başlatılıyor..."
docker compose up -d backend

echo "Sağlık kontrolü bekleniyor..."
for attempt in $(seq 1 10); do
    if curl -sf "$HEALTH_URL" >/dev/null; then
        echo "Rollback başarılı, servis sağlıklı: $HEALTH_URL"
        echo "=== Rollback Tamamlandı ==="
        exit 0
    fi
    sleep 2
done

echo "Rollback sonrası health check başarısız: $HEALTH_URL" >&2
exit 1
