@echo off
setlocal
cd /d "%~dp0"
echo === MOM Recorder setup ===
REM Only Python 3.10-3.13 work with the audio packages (3.14+ has no builds yet).
set PY=
for %%V in (3.12 3.11 3.13 3.10) do (
  if not defined PY ( py -%%V --version >nul 2>&1 && set PY=py -%%V )
)
if not defined PY (
  echo.
  echo No supported Python ^(3.10 - 3.13^) found. Your default Python is probably too new.
  echo Install Python 3.12 side by side ^(it will not affect your other Python^):
  echo    winget install Python.Python.3.12
  echo or download "Windows installer 64-bit" 3.12.x from https://www.python.org/downloads/windows/
  echo Then run this file again.
  pause & exit /b 1
)
echo Using: %PY%
%PY% --version
REM A half-finished or wrong-version environment is discarded.
if not exist .venv\.installed if exist .venv rmdir /s /q .venv
if not exist .venv\Scripts\python.exe (
  %PY% -m venv .venv || ( echo Could not create the virtual environment. & pause & exit /b 1 )
)
.venv\Scripts\python -m pip install --upgrade pip
.venv\Scripts\python -m pip install -r requirements.txt
if errorlevel 1 (
  echo.
  echo *** Package install FAILED - see the messages above. ***
  pause & exit /b 1
)
echo ok> .venv\.installed
echo.
echo Setup complete.
