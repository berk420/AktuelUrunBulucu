<#
.SYNOPSIS
    Backend'i önceki versiyona geri döndürür ve servisi yeniden başlatır.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$DeployPath   = "C:\deploy\backend"
$PreviousPath = "C:\deploy\backend-previous"
$ServiceName  = "AktuelUrunBulucu"

Write-Host "=== Rollback Başladı ==="

if (-not (Test-Path $PreviousPath)) {
    Write-Error "❌ Önceki versiyon bulunamadı: $PreviousPath"
    exit 1
}

# Servisi durdur
Write-Host "Servis durduruluyor: $ServiceName"
$service = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
if ($service -and $service.Status -ne "Stopped") {
    Stop-Service $ServiceName
    Start-Sleep -Seconds 2
}

# Mevcut versiyonu kaldır, öncekini geri yükle
if (Test-Path $DeployPath) {
    Remove-Item $DeployPath -Recurse -Force
}
Rename-Item $PreviousPath $DeployPath
Write-Host "Önceki versiyon geri yüklendi: $DeployPath"

# Servisi başlat
Write-Host "Servis başlatılıyor: $ServiceName"
Start-Service $ServiceName
Start-Sleep -Seconds 3

# Sağlık kontrolü
$HealthUrl = "http://localhost:5012/health"
try {
    $response = Invoke-WebRequest -Uri $HealthUrl -UseBasicParsing -TimeoutSec 10
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ Rollback başarılı, servis sağlıklı."
    } else {
        throw "Health check HTTP $($response.StatusCode)"
    }
} catch {
    Write-Error "❌ Rollback sonrası health check başarısız: $_"
    exit 1
}

Write-Host "=== Rollback Tamamlandı ==="
