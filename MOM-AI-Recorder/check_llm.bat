@echo off
cd /d "%~dp0"
if not exist .venv\.installed call install.bat
.venv\Scripts\python check_llm.py
pause
