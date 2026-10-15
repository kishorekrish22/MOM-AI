@echo off
cd /d "%~dp0"
if not exist .venv\.installed call install.bat
.venv\Scripts\python regenerate_mom.py %*
pause
