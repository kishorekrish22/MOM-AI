@echo off
cd /d "%~dp0"
if not exist .venv\.installed call install.bat
if not exist .venv\.installed exit /b 1
.venv\Scripts\python app.py
echo.
echo ---- app exited ^(see messages above; also crash.log in this folder^) ----
pause
