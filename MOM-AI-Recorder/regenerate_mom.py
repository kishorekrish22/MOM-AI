"""Build a MOM from an existing transcript .txt (e.g. after fixing the AI connection).
Usage: regenerate_mom.bat  (picks the newest transcript)   or   regenerate_mom.bat path\\to\\Transcript_x.txt"""
import re
import sys
from datetime import datetime
from pathlib import Path
from mom_recorder.config import load_config
from mom_recorder.mom_generator import generate_mom

cfg = load_config()
if len(sys.argv) > 1:
    f = Path(sys.argv[1])
else:
    f = max(cfg["_transcripts"].glob("Transcript_*.txt"), key=lambda p: p.stat().st_mtime)
print("Using", f)
text = f.read_text(encoding="utf-8")
m = re.search(r"(\d{4}-\d{2}-\d{2})_(\d{4})", f.name)
started = datetime.strptime(m.group(1) + m.group(2), "%Y-%m-%d%H%M")
ended = datetime.fromtimestamp(f.stat().st_mtime)
print("Created:", generate_mom(text, started, ended, cfg))
