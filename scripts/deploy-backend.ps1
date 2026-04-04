<#
.SYNOPSIS
    AktuelUrunBulucu backend'ini Windows Server'a deploy eder.
    Mevcut versiyonu yedekler, yenisini publish eder, servisi yeniden başlatır.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$BackendProject = "Backend\AktuelUrunBulucu\AktuelUrunBulucu.csproj"
$DeployPath     = "C:\deploy\backend"
$NewDeployPath  = "C:\deploy\backend-new"
$PreviousPath   = "C:\deploy\backend-previous"
$ServiceName    = "AktuelUrunBulucu"

Write-Host "=== Backend Deploy Başladı ==="

# Önceki versiyonu yedekle
if (Test-Path $DeployPath) {
    if (Test-Path $PreviousPath) {
        Remove-Item $PreviousPath -Recurse -Force
    }
    Write-Host "Mevcut versiyon yedekleniyor: $PreviousPath"
    Copy-Item $DeployPath $PreviousPath -Recurse
}

# Yeni build publish et
Write-Host "Publish yapılıyor..."
if (Test-Path $NewDeployPath) {
    Remove-Item $NewDeployPath -Recurse -Force
}
dotnet publish $BackendProject --configuration Release --output $NewDeployPath

# Servisi durdur
Write-Host "Servis durduruluyor: $ServiceName"
$service = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
if ($service -and $service.Status -ne "Stopped") {
    Stop-Service $ServiceName
    Start-Sleep -Seconds 2
}

# Swap
if (Test-Path $DeployPath) {
    Remove-Item $DeployPath -Recurse -Force
}
Rename-Item $NewDeployPath $DeployPath

# Servisi başlat
Write-Host "Servis başlatılıyor: $ServiceName"
Start-Service $ServiceName
Start-Sleep -Seconds 3

# Sağlık kontrolü
$HealthUrl = "http://localhost:5012/health"
try {
    $response = Invoke-WebRequest -Uri $HealthUrl -UseBasicParsing -TimeoutSec 10
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ Backend sağlıklı: $HealthUrl"
    } else {
        throw "Health check HTTP $($response.StatusCode)"
    }
} catch {
    Write-Error "❌ Health check başarısız: $_"
    exit 1
}

Write-Host "=== Backend Deploy Tamamlandı ==="
