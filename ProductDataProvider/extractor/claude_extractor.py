"""Product extractor that uses the Claude Vision API."""
import base64
import json
import mimetypes

import anthropic

import config
from extractor.base import BaseExtractor

_SYSTEM_PROMPT = (
    "Sen bir Türkçe market kataloğu analiz uzmanısın. "
    "Verilen görsel bir market aktüel kataloğudur. "
    "Görseldeki her ürünü tespit et ve aşağıdaki JSON formatında döndür. "
    "Yanıtın SADECE geçerli bir JSON dizisi olsun, başka hiçbir metin ekleme.\n\n"
    'Format: [{"name": "Ürün Adı", "category": "Kategori"}, ...]'
)

_USER_PROMPT = (
    "Bu market kataloğu görselindeki tüm ürünleri tespit et. "
    "Her ürün için ürün adını ve kategorisini çıkar. "
    "Yanıtın yalnızca JSON dizisi olsun."
)


class ClaudeExtractor(BaseExtractor):
    """Extracts product data from catalog images via the Claude Vision API."""

    def __init__(self) -> None:
        if not config.ANTHROPIC_API_KEY:
            raise ValueError(
                "ANTHROPIC_API_KEY environment variable is not set. "
                "Export it before running with --method claude."
            )
        self._client = anthropic.Anthropic(api_key=config.ANTHROPIC_API_KEY)

    def extract(self, image_path: str) -> list[dict]:
        """
        Send the image to Claude Vision and parse the returned JSON product list.

        Args:
            image_path: Path to the catalog image file.

        Returns:
            List of dicts with ``name`` and ``category`` keys.
        """
        media_type = self._detect_media_type(image_path)
        image_data = self._encode_image(image_path)

        message = self._client.messages.create(
            model=config.CLAUDE_MODEL,
            max_tokens=4096,
            system=_SYSTEM_PROMPT,
            messages=[
                {
                    "role": "user",
                    "content": [
                        {
                            "type": "image",
                            "source": {
                                "type": "base64",
                                "media_type": media_type,
                                "data": image_data,
                            },
                        },
                        {"type": "text", "text": _USER_PROMPT},
                    ],
                }
            ],
        )

        raw = message.content[0].text.strip()
        return self._parse_response(raw)

    # ------------------------------------------------------------------
    # Private helpers
    # ------------------------------------------------------------------

    @staticmethod
    def _encode_image(image_path: str) -> str:
        with open(image_path, "rb") as f:
            return base64.standard_b64encode(f.read()).decode("utf-8")

    @staticmethod
    def _detect_media_type(image_path: str) -> str:
        mime, _ = mimetypes.guess_type(image_path)
        supported = {"image/jpeg", "image/png", "image/gif", "image/webp"}
        if mime not in supported:
            return "image/jpeg"
        return mime

    @staticmethod
    def _parse_response(raw: str) -> list[dict]:
        # Strip markdown code fences if present
        if raw.startswith("```"):
            lines = raw.splitlines()
            raw = "\n".join(lines[1:-1] if lines[-1].strip() == "```" else lines[1:])

        products: list[dict] = json.loads(raw)

        validated: list[dict] = []
        for item in products:
            name = str(item.get("name", "")).strip()
            category = str(item.get("category", "")).strip()
            if name:
                validated.append({"name": name, "category": category or "Diğer"})
        return validated
