<#
.SYNOPSIS
    AktuelUrunBulucu frontend'ini build edip Nginx'e deploy eder.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$FrontendDir = "Frontend\AktuelUrunBulucu"
$NginxHtml   = "C:\nginx\html"
$NginxExe    = "C:\nginx\nginx.exe"

Write-Host "=== Frontend Deploy Başladı ==="

# Bağımlılıkları yükle ve build al
Set-Location $FrontendDir
Write-Host "npm ci..."
npm ci
Write-Host "npm run build..."
npm run build

# Nginx dizinine kopyala
Write-Host "Nginx dizinine kopyalanıyor: $NginxHtml"
if (-not (Test-Path $NginxHtml)) {
    New-Item -ItemType Directory -Path $NginxHtml | Out-Null
}
Remove-Item "$NginxHtml\*" -Recurse -Force -ErrorAction SilentlyContinue
Copy-Item "dist\*" $NginxHtml -Recurse

# Nginx'i yeniden yükle
Write-Host "Nginx reload..."
& $NginxExe -s reload
if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Nginx reload başarısız."
    exit 1
}

Write-Host "=== Frontend Deploy Tamamlandı ==="
