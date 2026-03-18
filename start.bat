@echo off
title Aktuel Urun Bulucu - Baslatici
color 0A
cls

echo ============================================
echo   Aktuel Urun Bulucu - Tam Baslati
echo ============================================
echo.

:: Git Bash uzerinden cagrildiginda da calissin
:: %~dp0 = bu bat dosyasinin bulundugu klasor (her zaman Windows yolu)
set "ROOT=%~dp0"
if "%ROOT:~-1%"=="\" set "ROOT=%ROOT:~0,-1%"

:: --- 1. Docker / Postgres ---
echo [1/5] Docker konteynerler baslatiliyor...
docker compose -f "%ROOT%\docker-compose.yml" up -d
if errorlevel 1 (
    echo [HATA] Docker baslatılamadı! Docker Desktop acik mi?
    pause
    exit /b 1
)
echo [OK] PostgreSQL ve pgAdmin calisiyor.
echo.

:: --- Postgres hazir olana kadar bekle (max 30 sn) ---
echo Veritabani hazir olana kadar bekleniyor...
set /a attempt=0
:wait_db
set /a attempt+=1
docker exec aktuel_db pg_isready -U aktuel_user -d aktueldb >nul 2>&1
if errorlevel 1 (
    if %attempt% geq 30 (
        echo [HATA] Veritabani 30 saniyede hazir olmadı!
        pause
        exit /b 1
    )
    timeout /t 1 /nobreak >nul
    goto wait_db
)
echo [OK] Veritabani hazir.
echo.

:: --- 2. .NET Backend ---
echo [2/5] .NET Backend baslatiliyor (port 5012)...
start "Backend - .NET" cmd.exe /k "cd /d "%ROOT%\Backend\AktuelUrunBulucu" && dotnet run"
echo [OK] Backend terminali acildi.
echo.

:: --- Backend ayaga kalkana kadar bekle (max 45 sn) ---
echo Backend baslayana kadar bekleniyor...
set /a attempt=0
:wait_backend
set /a attempt+=1
curl -s -o nul -w "%%{http_code}" http://localhost:5012/api/stores 2>nul | findstr "200" >nul
if errorlevel 1 (
    if %attempt% geq 45 (
        echo [UYARI] Backend 45sn icinde yanit vermedi, devam ediliyor...
        goto skip_backend_wait
    )
    timeout /t 1 /nobreak >nul
    goto wait_backend
)
:skip_backend_wait
echo [OK] Backend hazir.
echo.

:: --- 3. React Frontend ---
echo [3/5] React Frontend baslatiliyor (port 5173)...
start "Frontend - React" cmd.exe /k "cd /d "%ROOT%\Frontend\AktuelUrunBulucu" && npm run dev"
echo [OK] Frontend terminali acildi.
echo.

:: --- 4. Python Data Provider ---
echo [4/5] Python Veri Saglayici baslatiliyor (port 5050)...
start "DataProvider - Python" cmd.exe /k "cd /d "%ROOT%\ProductDataProvider" && python server.py"
echo [OK] Python server terminali acildi.
echo.

:: --- Servislerin hazirlanmasi icin bekle ---
echo [5/5] Servisler hazirlanıyor (6 sn)...
timeout /t 6 /nobreak >nul

:: --- Browser ---
echo Browser sekmeleri aciliyor...
start "" "http://localhost:5173"
timeout /t 1 /nobreak >nul
start "" "http://localhost:5050"
echo.

echo ============================================
echo   Tum servisler baslatildi!
echo.
echo   Ana Uygulama : http://localhost:5173
echo   Backend API  : http://localhost:5012
echo   Veri Yukleme : http://localhost:5050
echo   pgAdmin      : http://localhost:8080
echo ============================================
echo.
echo 3 ayri terminal penceresi acik olmali:
echo   - Backend - .NET
echo   - Frontend - React
echo   - DataProvider - Python
echo.
echo Hepsini durdurmak icin bu 3 terminali de kapatın.
echo.
pause
