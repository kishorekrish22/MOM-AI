"""One recording session: capture -> transcribe -> live transcript document."""
import logging
import queue
import threading
from dataclasses import dataclass
from datetime import datetime
from difflib import SequenceMatcher
from pathlib import Path

from .audio import AudioCapture
from .transcript_doc import TranscriptWriter

log = logging.getLogger(__name__)


@dataclass
class SessionResult:
    docx_path: Path
    txt_path: Path
    started: datetime
    ended: datetime
    text: str


class MeetingSession:
    def __init__(self, cfg, transcriber):
        self.cfg, self.transcriber = cfg, transcriber
        self.segq: "queue.Queue" = queue.Queue()
        self.started = datetime.now()
        self.writer = TranscriptWriter(cfg["_transcripts"], self.started)
        self.capture = AudioCapture(cfg, self.segq)
        self.thread = None
        self._recent_remote = []  # (t0, text) used to drop mic echo of the remote speakers

    def start(self):
        self.capture.start()
        self.thread = threading.Thread(target=self._loop, daemon=True, name="transcribe")
        self.thread.start()

    def stop(self) -> SessionResult:
        self.capture.stop()
        self.segq.put(None)
        self.thread.join()  # finishes transcribing whatever is still queued
        ended = datetime.now()
        self.writer.finalize(ended)
        return SessionResult(self.writer.docx_path, self.writer.txt_path,
                             self.started, ended, self.writer.text())

    def _is_echo(self, seg, text) -> bool:
        if seg.label != self.cfg["mic_label"]:
            return False
        return any(abs(seg.t0 - t) < 15 and SequenceMatcher(None, text.lower(), r.lower()).ratio() > 0.7
                   for t, r in self._recent_remote)

    def _loop(self):
        while True:
            seg = self.segq.get()
            if seg is None:
                return
            try:
                text = self.transcriber.transcribe(seg.audio)
            except Exception:
                log.exception("transcription failed")
                continue
            if not text:
                continue
            if seg.label == self.cfg["system_label"]:
                self._recent_remote = self._recent_remote[-20:] + [(seg.t0, text)]
            elif self._is_echo(seg, text):
                continue
            self.writer.add(datetime.fromtimestamp(seg.t0), seg.label, text)
