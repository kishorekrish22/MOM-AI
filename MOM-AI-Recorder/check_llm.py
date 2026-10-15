"""Diagnose the connection to the cloud-VM AI. Run via check_llm.bat"""
import sys
import requests
from mom_recorder.config import load_config

cfg = load_config()
base = cfg["ollama_url"].rsplit("/api/", 1)[0]
print("AI server:", base)
try:
    r = requests.get(base + "/api/tags", timeout=20)
    print("GET /api/tags ->", r.status_code)
    models = [m["name"] for m in r.json().get("models", [])]
    print("Models on the VM:", models or "NONE")
except Exception as e:
    print("FAILED to reach the server:", e)
    print("-> VM stopped? Port 11434 blocked by the Google Cloud firewall / your network? Port-forward closed?")
    sys.exit(1)
model = cfg["llm_model"] or (models[0] if models else "")
if not model:
    sys.exit("No model available - set llm_model in config.json")
print("Test generation with model:", model)
try:
    r = requests.post(cfg["ollama_url"], json={"model": model, "prompt": "Reply with the word OK.",
                                                "stream": False}, timeout=180)
    print("HTTP", r.status_code, "->", r.text[:300])
except Exception as e:
    print("Generation FAILED:", e)
