@echo off
title Aktuel Urun Bulucu - Baslatici
color 0A
cls

echo ============================================
echo   Aktuel Urun Bulucu - Tam Baslati
echo ============================================
echo.

set "ROOT=%~dp0"
if "%ROOT:~-1%"=="\" set "ROOT=%ROOT:~0,-1%"

set "DOCKER_DESKTOP=C:\Program Files\Docker\Docker\Docker Desktop.exe"
set "DEVENV=C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\devenv.exe"
set "SOLUTION=%ROOT%\Backend\AktuelUrunBulucu\AktuelUrunBulucu.slnx"
set "GITHUB_DESKTOP=C:\Users\berk\AppData\Local\GitHubDesktop\GitHubDesktop.exe"

:: --- 1. Docker Desktop ---
echo [1/5] Docker Desktop kontrol ediliyor...
docker info >nul 2>&1
if errorlevel 1 (
    echo Docker Desktop aciliyor, lutfen bekleyin...
    start "" "%DOCKER_DESKTOP%"
    :wait_docker
    timeout /t 3 /nobreak >nul
    docker info >nul 2>&1
    if errorlevel 1 goto wait_docker
    echo [OK] Docker Desktop hazir.
) else (
    echo [OK] Docker Desktop zaten calisiyor.
)
echo.

:: --- 2. Docker / Postgres ---
echo [2/5] Docker konteynerler baslatiliyor...
docker compose -f "%ROOT%\docker-compose.yml" up -d
if errorlevel 1 (
    echo [HATA] Docker baslatılamadı!
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

:: --- 3. Visual Studio ---
echo [3/5] Visual Studio aciliyor...
start "" "%DEVENV%" "%SOLUTION%"
echo [OK] Visual Studio acildi.
echo.

:: --- GitHub Desktop ---
echo GitHub Desktop aciliyor...
start "" "%GITHUB_DESKTOP%"
echo [OK] GitHub Desktop acildi.
echo.

:: --- 4. React Frontend ---
echo [4/5] React Frontend baslatiliyor (port 5173)...
start "Frontend - React" cmd.exe /k "cd /d "%ROOT%\Frontend\AktuelUrunBulucu" && npm run dev"
echo [OK] Frontend terminali acildi.
echo.

:: --- 5. Python Data Provider ---
echo [5/5] Python Veri Saglayici baslatiliyor (port 5050)...
start "DataProvider - Python" cmd.exe /k "cd /d "%ROOT%\ProductDataProvider" && python server.py"
echo [OK] Python server terminali acildi.
echo.

:: --- Servislerin hazirlanmasi icin bekle ---
echo Servisler hazirlanıyor (6 sn)...
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
pause
