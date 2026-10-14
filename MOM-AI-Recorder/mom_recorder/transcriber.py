"""Speech-to-text with faster-whisper (open-source Whisper)."""
import logging
import threading

log = logging.getLogger(__name__)

# Whisper tends to hallucinate these on near-silence.
HALLUCINATIONS = {"thank you.", "thanks for watching!", "thank you for watching.", "you", "bye.", "."}


class Transcriber:
    def __init__(self, cfg: dict):
        self.cfg = cfg
        self.model = None
        self._lock = threading.Lock()
        self.ready = threading.Event()

    def load(self):
        with self._lock:
            if self.model is not None:
                return
            from faster_whisper import WhisperModel
            c = self.cfg
            log.info("Loading Whisper model '%s' (first run downloads it)...", c["whisper_model"])
            self.model = WhisperModel(c["whisper_model"], device=c["whisper_device"],
                                      compute_type=c["whisper_compute_type"])
            self.ready.set()
            log.info("Whisper model ready")

    def preload(self, on_done=None):
        def run():
            try:
                self.load()
                if on_done:
                    on_done(None)
            except Exception as e:
                log.exception("Whisper load failed")
                if on_done:
                    on_done(e)
        threading.Thread(target=run, daemon=True, name="whisper-load").start()

    def transcribe(self, audio) -> str:
        self.load()
        segs, _ = self.model.transcribe(
            audio, language=self.cfg["language"] or None, beam_size=self.cfg["beam_size"],
            vad_filter=True, condition_on_previous_text=False)
        parts = []
        for s in segs:
            if s.no_speech_prob > 0.7 and s.avg_logprob < -1.0:
                continue
            t = s.text.strip()
            if t and t.lower() not in HALLUCINATIONS:
                parts.append(t)
        return " ".join(parts)
