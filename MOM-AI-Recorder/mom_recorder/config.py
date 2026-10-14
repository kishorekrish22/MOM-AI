"""Configuration + well-known folders."""
import json
import os
import sys
from pathlib import Path

DEFAULTS = {
    "ollama_url": "http://35.200.209.120:11434/api/generate",
    "llm_model": "",
    "llm_timeout_s": 900,
    "whisper_model": "small",
    "whisper_device": "auto",
    "whisper_compute_type": "auto",
    "language": "en",
    "beam_size": 1,
    "capture_microphone": True,
    "capture_system_audio": True,
    "mic_device_contains": "",
    "system_device_contains": "",
    "mic_label": "You",
    "system_label": "Meeting",
    "silence_threshold": 0.008,
    "pause_s": 0.8,
    "max_segment_s": 25,
    "own_organization": "Mahat",
    "default_mode": "Online - Microsoft Teams",
    "output_root": "",
}


def app_dir() -> Path:
    if getattr(sys, "frozen", False):
        return Path(sys.executable).parent
    return Path(__file__).resolve().parent.parent


def resource_path(rel: str) -> Path:
    base = Path(getattr(sys, "_MEIPASS", app_dir()))
    return base / rel


def config_path() -> Path:
    return app_dir() / "config.json"


def documents_dir() -> Path:
    """The user's real Documents folder (handles OneDrive-redirected Documents on Windows)."""
    if os.name == "nt":
        try:
            import ctypes
            from ctypes import wintypes

            class GUID(ctypes.Structure):
                _fields_ = [("a", wintypes.DWORD), ("b", wintypes.WORD),
                            ("c", wintypes.WORD), ("d", wintypes.BYTE * 8)]

            guid = GUID(0xFDD39AD0, 0x238F, 0x46AF,
                        (wintypes.BYTE * 8)(0xAD, 0xB4, 0x6C, 0x85, 0x48, 0x03, 0x69, 0xC7))
            path = ctypes.c_wchar_p()
            if ctypes.windll.shell32.SHGetKnownFolderPath(ctypes.byref(guid), 0, None,
                                                          ctypes.byref(path)) == 0:
                p = Path(path.value)
                ctypes.windll.ole32.CoTaskMemFree(path)
                return p
        except Exception:
            pass
    return Path.home() / "Documents"


def load_config() -> dict:
    cfg = dict(DEFAULTS)
    p = config_path()
    if p.exists():
        with open(p, encoding="utf-8") as f:
            cfg.update(json.load(f))
    root = Path(cfg["output_root"]) if cfg["output_root"] else documents_dir() / "MOM-AI"
    cfg["_root"] = root
    cfg["_transcripts"] = root / "Transcripts"
    cfg["_moms"] = root / "MOMs"
    for d in (root, cfg["_transcripts"], cfg["_moms"]):
        d.mkdir(parents=True, exist_ok=True)
    return cfg
