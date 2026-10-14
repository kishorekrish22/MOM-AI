"""MOM Recorder - system-tray app. Toggle the mic icon to record a meeting; on stop it builds the MOM."""
import logging
import os
import subprocess
import sys
import threading
from datetime import datetime

import pystray
from PIL import Image, ImageDraw

from mom_recorder.config import config_path, load_config
from mom_recorder.mom_generator import generate_mom
from mom_recorder.session import MeetingSession
from mom_recorder.transcriber import Transcriber

log = logging.getLogger("mom")


def make_icon(recording: bool) -> Image.Image:
    img = Image.new("RGBA", (64, 64), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    d.ellipse((2, 2, 62, 62), fill=(200, 30, 40, 255) if recording else (45, 55, 72, 255))
    w = (255, 255, 255, 255)
    d.rounded_rectangle((24, 12, 40, 36), radius=8, fill=w)      # capsule
    d.arc((17, 20, 47, 46), 0, 180, fill=w, width=3)              # cradle
    d.line((32, 46, 32, 53), fill=w, width=3)                     # stem
    d.line((24, 53, 40, 53), fill=w, width=3)                     # base
    return img


def open_path(path):
    path = str(path)
    if os.name == "nt":
        os.startfile(path)
    else:
        subprocess.Popen(["open" if sys.platform == "darwin" else "xdg-open", path])


class App:
    def __init__(self):
        self.cfg = load_config()
        self.transcriber = Transcriber(self.cfg)
        self.session = None
        self.busy = False  # generating MOM
        self.last_mom = None
        self.icon = pystray.Icon(
            "mom-recorder", make_icon(False), "MOM Recorder - idle",
            menu=pystray.Menu(
                pystray.MenuItem(lambda i: "Stop recording" if self.session else "Start recording",
                                 self.toggle, default=True),
                pystray.MenuItem("Open last MOM", lambda i, it: open_path(self.last_mom),
                                 enabled=lambda i: bool(self.last_mom)),
                pystray.MenuItem("Open output folder", lambda i, it: open_path(self.cfg["_root"])),
                pystray.MenuItem("Edit settings (config.json)", lambda i, it: open_path(config_path())),
                pystray.Menu.SEPARATOR,
                pystray.MenuItem("Quit", self.quit)))

    def notify(self, msg, title="MOM Recorder"):
        log.info("%s: %s", title, msg)
        try:
            self.icon.notify(msg, title)
        except Exception:
            pass

    def set_state(self, recording, tip):
        self.icon.icon = make_icon(recording)
        self.icon.title = tip
        self.icon.update_menu()

    def toggle(self, icon=None, item=None):
        if self.busy:
            self.notify("Still generating the previous MOM - please wait.")
            return
        if self.session is None:
            self.start()
        else:
            self.stop()

    def start(self):
        try:
            s = MeetingSession(self.cfg, self.transcriber)
            s.start()
        except Exception as e:
            log.exception("start failed")
            self.notify(f"Could not start recording: {e}")
            return
        self.session = s
        self.set_state(True, "MOM Recorder - RECORDING (click to stop)")
        self.notify("Recording started (microphone + system audio).")
        if not self.transcriber.ready.is_set():
            self.notify("Whisper model is still loading - audio is queued and will appear shortly.")

    def stop(self):
        s, self.session = self.session, None
        self.busy = True
        self.set_state(False, "MOM Recorder - finishing...")
        threading.Thread(target=self._finish, args=(s,), daemon=True).start()

    def _finish(self, s):
        try:
            self.notify("Recording stopped. Finishing transcript...")
            res = s.stop()
            self.notify(f"Transcript saved: {res.docx_path.name}\nGenerating MOM...")
            try:
                mom = generate_mom(res.text, res.started, res.ended, self.cfg)
                self.last_mom = mom
                self.notify(f"MOM ready: {mom.name}")
            except Exception as e:
                log.exception("MOM generation failed")
                self.notify(f"MOM generation failed ({e}). Transcript is saved in Documents/MOM-AI.")
        except Exception as e:
            log.exception("stop failed")
            self.notify(f"Error while stopping: {e}")
        finally:
            self.busy = False
            self.set_state(False, "MOM Recorder - idle")

    def quit(self, icon=None, item=None):
        if self.session:
            try:
                self.session.stop()
            except Exception:
                log.exception("stop on quit")
        self.icon.stop()

    def run(self):
        self.transcriber.preload(lambda err: self.notify(
            "Ready - click the mic icon to start recording." if err is None
            else f"Whisper failed to load: {err}"))
        self.icon.run()


def main():
    cfg_root = load_config()["_root"]
    logging.basicConfig(level=logging.INFO, format="%(asctime)s %(levelname)s %(name)s: %(message)s",
                        handlers=[logging.FileHandler(cfg_root / "app.log", encoding="utf-8"),
                                  logging.StreamHandler()])
    App().run()


if __name__ == "__main__":
    main()
