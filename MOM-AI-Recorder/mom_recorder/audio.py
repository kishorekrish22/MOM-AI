"""Microphone + system-audio (WASAPI loopback) capture with silence-based segmentation."""
import logging
import queue
import threading
import time
from collections import deque
from dataclasses import dataclass

import numpy as np

log = logging.getLogger(__name__)
TARGET_SR = 16000


def to_16k(x: np.ndarray, sr: int) -> np.ndarray:
    """Mono float32 -> 16 kHz (box low-pass + linear interpolation; plenty for speech)."""
    if sr == TARGET_SR or len(x) == 0:
        return x.astype(np.float32)
    ratio = sr / TARGET_SR
    if ratio > 1:
        w = max(1, int(round(ratio)))
        x = np.convolve(x, np.ones(w, dtype=np.float32) / w, mode="same")
    idx = np.arange(0, len(x), ratio)
    return np.interp(idx, np.arange(len(x)), x).astype(np.float32)


@dataclass
class Segment:
    label: str
    audio: np.ndarray  # 16 kHz mono float32
    t0: float          # wall-clock start


class Segmenter:
    """Cuts a continuous stream into utterances at pauses (or at max length)."""

    def __init__(self, label, rate, threshold, pause_s, max_s, min_s=0.4):
        self.label, self.rate = label, rate
        self.threshold, self.pause_s, self.max_s, self.min_s = threshold, pause_s, max_s, min_s
        self.preroll = deque(maxlen=4)
        self.buf, self.active = [], False
        self.n = self.silence = 0
        self.t0 = self.last_feed = 0.0

    def _emit(self):
        if not self.buf:
            return None
        audio = np.concatenate(self.buf)
        t0, self.buf, self.active, self.n, self.silence = self.t0, [], False, 0, 0
        if len(audio) < self.min_s * self.rate:
            return None
        return Segment(self.label, to_16k(audio, self.rate), t0)

    def feed(self, block: np.ndarray, now: float):
        self.last_feed = now
        speech = float(np.sqrt(np.mean(block ** 2))) > self.threshold if len(block) else False
        if not self.active:
            self.preroll.append(block)
            if speech:
                self.active, self.buf = True, list(self.preroll)
                self.n, self.silence = sum(len(b) for b in self.buf), 0
                self.t0 = now - self.n / self.rate
                self.preroll.clear()
            return None
        self.buf.append(block)
        self.n += len(block)
        self.silence = 0 if speech else self.silence + len(block)
        if self.silence >= self.pause_s * self.rate or self.n >= self.max_s * self.rate:
            return self._emit()
        return None

    def idle_flush(self, now: float):
        # WASAPI loopback delivers nothing while the PC is silent, so flush on a timer too.
        if self.active and now - self.last_feed > self.pause_s:
            return self._emit()
        return None

    def flush(self):
        return self._emit() if self.active else None


class AudioCapture:
    def __init__(self, cfg: dict, segq: "queue.Queue"):
        self.cfg, self.segq = cfg, segq
        self.rawq: "queue.Queue" = queue.Queue()
        self.stop_evt = threading.Event()
        self.pa, self.streams, self.worker = None, [], None
        self.sources = []

    # -- device discovery -------------------------------------------------
    def _devices(self):
        import pyaudiowpatch as pyaudio
        self.pyaudio = pyaudio
        self.pa = pyaudio.PyAudio()
        wasapi = self.pa.get_host_api_info_by_type(pyaudio.paWASAPI)
        out = []
        if self.cfg["capture_microphone"]:
            want = self.cfg["mic_device_contains"].lower()
            dev = None
            if want:
                dev = next((d for d in self._all() if not d.get("isLoopbackDevice")
                            and d["maxInputChannels"] > 0 and want in d["name"].lower()), None)
            dev = dev or self.pa.get_device_info_by_index(wasapi["defaultInputDevice"])
            out.append((self.cfg["mic_label"], dev))
        if self.cfg["capture_system_audio"]:
            want = self.cfg["system_device_contains"].lower()
            lbs = list(self.pa.get_loopback_device_info_generator())
            dev = next((d for d in lbs if want and want in d["name"].lower()), None)
            if dev is None:
                spk = self.pa.get_device_info_by_index(wasapi["defaultOutputDevice"])
                dev = spk if spk.get("isLoopbackDevice") else next(
                    (d for d in lbs if spk["name"] in d["name"]), lbs[0] if lbs else None)
            if dev is None:
                log.warning("No loopback device found - system audio will not be captured")
            else:
                out.append((self.cfg["system_label"], dev))
        return out

    def _all(self):
        return [self.pa.get_device_info_by_index(i) for i in range(self.pa.get_device_count())]

    # -- lifecycle --------------------------------------------------------
    def start(self):
        self.sources = self._devices()
        if not self.sources:
            raise RuntimeError("No audio sources enabled/found")
        pa = self.pyaudio
        for label, dev in self.sources:
            ch, rate = int(dev["maxInputChannels"]), int(dev["defaultSampleRate"])
            log.info("Capturing '%s' from: %s (%d ch @ %d Hz)", label, dev["name"], ch, rate)

            def cb(data, frames, tinfo, status, label=label, ch=ch, rate=rate):
                self.rawq.put((label, data, ch, rate))
                return (None, pa.paContinue)

            st = self.pa.open(format=pa.paInt16, channels=ch, rate=rate, input=True,
                              input_device_index=dev["index"], frames_per_buffer=1024,
                              stream_callback=cb)
            self.streams.append(st)
        self.worker = threading.Thread(target=self._work, daemon=True, name="audio-worker")
        self.worker.start()

    def stop(self):
        for st in self.streams:
            try:
                st.stop_stream()
                st.close()
            except Exception:
                log.exception("closing stream")
        self.streams = []
        self.stop_evt.set()
        if self.worker:
            self.worker.join(timeout=30)
        if self.pa:
            self.pa.terminate()
            self.pa = None

    def _work(self):
        c, segs = self.cfg, {}
        while True:
            try:
                label, data, ch, rate = self.rawq.get(timeout=0.3)
                x = np.frombuffer(data, dtype=np.int16).astype(np.float32) / 32768.0
                if ch > 1:
                    x = x[: len(x) // ch * ch].reshape(-1, ch).mean(axis=1)
                if label not in segs:
                    segs[label] = Segmenter(label, rate, c["silence_threshold"],
                                            c["pause_s"], c["max_segment_s"])
                s = segs[label].feed(x, time.time())
                if s:
                    self.segq.put(s)
            except queue.Empty:
                if self.stop_evt.is_set():
                    break
            now = time.time()
            for sg in segs.values():
                s = sg.idle_flush(now)
                if s:
                    self.segq.put(s)
        for sg in segs.values():
            s = sg.flush()
            if s:
                self.segq.put(s)
