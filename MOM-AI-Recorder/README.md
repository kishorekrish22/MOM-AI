# MOM Recorder

A Windows system-tray app. Click the **mic icon** to start recording your microphone **and** system audio
(e.g. a Teams call). Speech is transcribed live (open-source Whisper via `faster-whisper`) into a Word
document. When you stop, the transcript is sent to your cloud-VM LLM and a Minutes of the Meeting `.docx` is
produced from your organisation's MOM template (`templates/MOM_Template.docx`).

## Install & run (Windows 10/11, Python 3.10+)
1. Double-click `run.bat` (first run: `install.bat` creates a venv and installs packages; Python 3.11/3.12 recommended). If the icon does not appear, run `run_debug.bat` to see the error.
   The first start downloads the Whisper `small` model (~460 MB).
2. Look for the mic icon in the taskbar tray (click `^` and drag it out to pin it).
3. **Left-click** the icon (or right-click -> *Start recording*) to start; the icon turns **red**.
   Click again to stop. A toast tells you when the transcript and the MOM are ready.

Optional: put a shortcut to `run.bat` in `shell:startup` to launch at login.

## Output (`Documents\MOM-AI\`)
- `Transcripts\Transcript_<date>_<time>.docx` - updated live (plus a `.txt` copy). If you keep it open in
  Word, updates resume once you close it; the `.txt` is always current.
- `MOMs\MOM_<date>_<time>.docx` (+ `.json` of what the LLM returned)
- `app.log`

## How audio is captured
- Mic = default input device, labelled **You**. System audio = WASAPI loopback of the default output
  device, labelled **Meeting** - this is what Teams plays, no virtual cable needed.
- If Teams uses a different speaker/headset than the Windows default, set `system_device_contains` /
  `mic_device_contains` in `config.json` to part of the device name.
- With open speakers the mic re-records the remote voices; duplicates are filtered, but a headset is best.

## config.json
`ollama_url` (your VM endpoint, JSON `/api/generate`), `llm_model` (blank = first model the VM reports on
`/api/tags`; set it explicitly if you have several), `whisper_model` (`tiny/base/small/medium/large-v3`; use
`medium` on a GPU/fast CPU), `language`, `own_organization`, `default_mode`, `silence_threshold` (raise if
background noise is transcribed), `output_root`.

## What the AI fills in
Overview, scope, discussion points (numbered, title + paragraph), action items (activity / organisation /
owner / due by), and participants/mode **only if said in the meeting**; otherwise "Not captured" or the
`default_mode`. Date and time come from the recording. Please review every MOM before sending.

## Tests
`python -m unittest discover -s tests` (uses a fake Ollama server; no audio hardware or Whisper needed).

## Notes
- Transcripts are sent to the VM over plain HTTP. If that port is open to the internet, restrict it to your
  office IP or put it behind a VPN/TLS, as meeting content is confidential.
- Please tell attendees the meeting is being recorded/transcribed, per your company policy.
