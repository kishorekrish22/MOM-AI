@echo off
cd /d "%~dp0"
if not exist .venv\.installed call install.bat
if not exist .venv\.installed exit /b 1
start "" .venv\Scripts\pythonw.exe app.py
echo MOM Recorder started - look for the mic icon in the system tray ^(click the ^^ arrow near the clock^).
echo If it does not appear in ~10 seconds, run run_debug.bat to see the error.
timeout /t 8 >nul
