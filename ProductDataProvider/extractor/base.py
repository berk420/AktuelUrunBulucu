"""Abstract base class for all product extractors."""
from abc import ABC, abstractmethod


class BaseExtractor(ABC):
    """Base class that every extractor must implement."""

    @abstractmethod
    def extract(self, image_path: str) -> list[dict]:
        """
        Extract product data from a catalog image.

        Args:
            image_path: Absolute or relative path to the image file.

        Returns:
            A list of dicts, each with keys ``name`` (str) and ``category`` (str).
            Example::

                [
                    {"name": "Darbeli Matkap 600W", "category": "Araç Gereç"},
                    {"name": "Avuç Taşlama 750W",   "category": "Araç Gereç"},
                ]
        """
