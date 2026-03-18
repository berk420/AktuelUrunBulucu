"""Product extractor that uses EasyOCR (offline, free)."""
import re

from extractor.base import BaseExtractor

# Patterns that indicate a line is likely a product name (not a price, header, etc.)
_PRICE_PATTERN = re.compile(r"^\d[\d\s]*[,\.]\d{2}\s*(TL|₺)?$", re.IGNORECASE)
_SHORT_LINE = 3  # characters — skip lines shorter than this

# Common Turkish catalog category keywords → map to a category label
_CATEGORY_KEYWORDS: list[tuple[re.Pattern, str]] = [
    (re.compile(r"elektr(ik|onk|o)", re.IGNORECASE), "Elektrik & Elektronik"),
    (re.compile(r"mutfak|tencere|tava|çaydanlık", re.IGNORECASE), "Mutfak"),
    (re.compile(r"temizlik|deterjan|sabun", re.IGNORECASE), "Temizlik"),
    (re.compile(r"tekstil|havlu|çarşaf|nevresim", re.IGNORECASE), "Tekstil"),
    (re.compile(r"bahçe|bitki|tohum", re.IGNORECASE), "Bahçe"),
    (re.compile(r"gıda|yiyecek|içecek|süt|peynir|ekmek", re.IGNORECASE), "Gıda"),
    (re.compile(r"oyuncak|oyun|çocuk", re.IGNORECASE), "Oyuncak"),
    (re.compile(r"spor|fitness|bisiklet", re.IGNORECASE), "Spor"),
    (re.compile(r"matkap|testere|tornavida|alet|takım", re.IGNORECASE), "Araç Gereç"),
    (re.compile(r"mobilya|raf|dolap|masa|sandalye", re.IGNORECASE), "Mobilya"),
]


def _guess_category(text: str) -> str:
    for pattern, label in _CATEGORY_KEYWORDS:
        if pattern.search(text):
            return label
    return "Diğer"


class EasyOcrExtractor(BaseExtractor):
    """Extracts product names from catalog images using EasyOCR (Turkish language)."""

    def __init__(self) -> None:
        try:
            import easyocr  # noqa: PLC0415  (lazy import — may not be installed)
        except ImportError as exc:
            raise ImportError(
                "easyocr is not installed. Run: pip install easyocr"
            ) from exc

        # gpu=False keeps it portable; set to True if you have CUDA
        self._reader = easyocr.Reader(["tr", "en"], gpu=False)

    def extract(self, image_path: str) -> list[dict]:
        """
        Run OCR on the image and heuristically parse product names.

        Args:
            image_path: Path to the catalog image file.

        Returns:
            List of dicts with ``name`` and ``category`` keys.
        """
        results = self._reader.readtext(image_path, detail=0, paragraph=True)
        return self._parse_lines(results)

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
            # Skip lines that are all digits / symbols
            if re.match(r"^[\d\s\W]+$", line):
                continue
            products.append({"name": line, "category": _guess_category(line)})
        return products
