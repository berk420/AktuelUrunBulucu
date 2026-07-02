#!/usr/bin/env bash
# AktuelUrunBulucu backend'ini Ubuntu sunucuda Docker container olarak deploy eder.
# Mevcut image'ı "previous" olarak etiketler, yenisini build edip başlatır, health check yapar.
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT_DIR"

IMAGE_NAME="aktuel-backend"
HEALTH_URL="http://localhost:5012/health"

echo "=== Backend Deploy Başladı ==="

if docker image inspect "${IMAGE_NAME}:latest" >/dev/null 2>&1; then
    echo "Mevcut image yedekleniyor: ${IMAGE_NAME}:previous"
    docker tag "${IMAGE_NAME}:latest" "${IMAGE_NAME}:previous"
fi

echo "Yeni image build ediliyor..."
docker compose build backend

echo "Container yeniden başlatılıyor..."
docker compose up -d backend

echo "Sağlık kontrolü bekleniyor..."
for attempt in $(seq 1 10); do
    if curl -sf "$HEALTH_URL" >/dev/null; then
        echo "Backend sağlıklı: $HEALTH_URL"
        echo "=== Backend Deploy Tamamlandı ==="
        exit 0
    fi
    sleep 2
done

echo "Health check başarısız: $HEALTH_URL" >&2
exit 1
