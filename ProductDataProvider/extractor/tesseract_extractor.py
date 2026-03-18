"""Product extractor that uses Tesseract OCR via pytesseract (offline, free)."""
import re

from extractor.base import BaseExtractor
from extractor.easyocr_extractor import _PRICE_PATTERN, _SHORT_LINE, _guess_category

_TESSERACT_LANG = "tur+eng"


class TesseractExtractor(BaseExtractor):
    """Extracts product names from catalog images using Tesseract OCR."""

    def __init__(self) -> None:
        try:
            import pytesseract  # noqa: PLC0415
            from PIL import Image  # noqa: PLC0415, F401
        except ImportError as exc:
            raise ImportError(
                "pytesseract and Pillow are not installed. "
                "Run: pip install pytesseract Pillow\n"
                "Also make sure Tesseract is installed on your system."
            ) from exc

        self._pytesseract = pytesseract

    def extract(self, image_path: str) -> list[dict]:
        """
        Run Tesseract OCR on the image and heuristically parse product names.

        Args:
            image_path: Path to the catalog image file.

        Returns:
            List of dicts with ``name`` and ``category`` keys.
        """
        from PIL import Image  # noqa: PLC0415
        from pytesseract import TesseractNotFoundError  # noqa: PLC0415

        image = Image.open(image_path)
        try:
            raw_text: str = self._pytesseract.image_to_string(image, lang=_TESSERACT_LANG)
        except TesseractNotFoundError:
            raise RuntimeError(
                "Tesseract sisteminizde kurulu değil.\n\n"
                "Kurulum adımları:\n"
                "  1. https://github.com/UB-Mannheim/tesseract/wiki adresinden "
                "Windows installer'ı indirin\n"
                "  2. Kurulum sırasında 'Additional language data' → 'Turkish' seçin\n"
                "  3. Kurulum dizinini (örn. C:\\Program Files\\Tesseract-OCR) "
                "sistem PATH'ine ekleyin\n"
                "  4. Terminali yeniden başlatın ve tekrar deneyin\n\n"
                "Alternatif: --method claude veya --method easyocr kullanın."
            )
        lines = raw_text.splitlines()
        return self._parse_lines(lines)

    # ------------------------------------------------------------------
    # Private helpers
    # ------------------------------------------------------------------

    @staticmethod
    def _parse_lines(lines: list[str]) -> list[dict]:
        products: list[dict] = []
        for line in lines:
            line = line.strip()
            if len(line) < _SHORT_LINE:
                continue
            if _PRICE_PATTERN.match(line):
                continue
            if re.match(r"^[\d\s\W]+$", line):
                continue
            products.append({"name": line, "category": _guess_category(line)})
        return products
