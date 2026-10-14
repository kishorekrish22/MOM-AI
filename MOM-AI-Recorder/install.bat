@echo off
setlocal
cd /d "%~dp0"
echo === MOM Recorder setup ===
set PY=
py -3.12 --version >nul 2>&1 && set PY=py -3.12
if not defined PY ( py -3.11 --version >nul 2>&1 && set PY=py -3.11 )
if not defined PY ( py -3 --version >nul 2>&1 && set PY=py -3 )
if not defined PY ( python --version >nul 2>&1 && set PY=python )
if not defined PY (
  echo Python was not found. Install Python 3.11 or 3.12 from python.org ^(tick "Add Python to PATH"^), then run this again.
  pause & exit /b 1
)
echo Using: %PY%
%PY% --version
if exist .venv\Scripts\python.exe (
  echo Existing .venv found.
) else (
  %PY% -m venv .venv || ( echo Could not create the virtual environment. & pause & exit /b 1 )
)
.venv\Scripts\python -m pip install --upgrade pip
.venv\Scripts\python -m pip install -r requirements.txt
if errorlevel 1 (
  echo.
  echo *** Package install FAILED - see the messages above. Python 3.11 or 3.12 is recommended. ***
  pause & exit /b 1
)
echo ok> .venv\.installed
echo.
echo Setup complete.
