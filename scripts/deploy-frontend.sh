#!/usr/bin/env bash
# AktuelUrunBulucu frontend'ini build edip Nginx'in servis ettiği dizine deploy eder.
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
FRONTEND_DIR="$ROOT_DIR/Frontend/AktuelUrunBulucu"
TARGET_DIR="/var/www/aktuelurunbulucu/frontend"

echo "=== Frontend Deploy Başladı ==="

cd "$FRONTEND_DIR"
echo "npm ci..."
npm ci
echo "npm run build..."
npm run build

echo "Hedef dizine kopyalanıyor: $TARGET_DIR"
sudo mkdir -p "$TARGET_DIR"
sudo rm -rf "${TARGET_DIR:?}"/*
sudo cp -r dist/* "$TARGET_DIR/"

echo "Nginx reload..."
sudo nginx -t
sudo systemctl reload nginx

echo "=== Frontend Deploy Tamamlandı ==="
