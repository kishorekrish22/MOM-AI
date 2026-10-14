@echo off
REM Optional: build a standalone MOM-Recorder.exe (config.json is read from next to the exe)
cd /d "%~dp0"
if not exist .venv ( py -3 -m venv .venv && .venv\Scripts\pip install -r requirements.txt )
.venv\Scripts\pip install pyinstaller
.venv\Scripts\pyinstaller --noconsole --onedir --name MOM-Recorder --collect-all faster_whisper --collect-all ctranslate2 --add-data "templates;templates" app.py
copy /y config.json dist\MOM-Recorder\config.json
