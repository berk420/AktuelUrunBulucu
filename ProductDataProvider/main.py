"""
ProductDataProvider — CLI entry point.

Usage examples
--------------
# Claude Vision (single image)
python main.py --image images/bim_mart.jpg --date 2026-03-13 --store BİM --method claude

# EasyOCR (single image)
python main.py --image images/a101.jpg --date 2026-04-01 --store A101 --method easyocr

# Tesseract (single image)
python main.py --image images/migros.jpg --date 2026-03-13 --store Migros --method tesseract

# Whole folder (Claude Vision)
python main.py --folder images/ --date 2026-03-13 --store BİM --method claude

# Dry-run (no DB writes)
python main.py --image images/bim_mart.jpg --date 2026-03-13 --store BİM --method claude --dry-run
"""
import argparse
import json
import sys
from datetime import datetime
from pathlib import Path

import config
import db_writer
from extractor.base import BaseExtractor


def _build_extractor(method: str) -> BaseExtractor:
    if method == "claude":
        from extractor.claude_extractor import ClaudeExtractor
        return ClaudeExtractor()
    if method == "easyocr":
        from extractor.easyocr_extractor import EasyOcrExtractor
        return EasyOcrExtractor()
    if method == "tesseract":
        from extractor.tesseract_extractor import TesseractExtractor
        return TesseractExtractor()
    raise ValueError(f"Unknown method: {method!r}. Choose claude | easyocr | tesseract")


def _collect_images(args: argparse.Namespace) -> list[Path]:
    if args.image:
        path = Path(args.image)
        if not path.exists():
            print(f"[ERROR] Image not found: {path}", file=sys.stderr)
            sys.exit(1)
        return [path]

    folder = Path(args.folder)
    if not folder.is_dir():
        print(f"[ERROR] Folder not found: {folder}", file=sys.stderr)
        sys.exit(1)

    images = [p for p in folder.iterdir() if p.suffix.lower() in config.IMAGE_EXTENSIONS]
    if not images:
        print(f"[ERROR] No supported images found in {folder}", file=sys.stderr)
        sys.exit(1)
    return sorted(images)


def _parse_date(date_str: str) -> datetime:
    try:
        return datetime.strptime(date_str, "%Y-%m-%d")
    except ValueError:
        print(f"[ERROR] Invalid date format: {date_str!r}. Expected YYYY-MM-DD.", file=sys.stderr)
        sys.exit(1)


def _process_image(
    extractor: BaseExtractor,
    image_path: Path,
    store_name: str,
    bring_date: datetime,
    dry_run: bool,
) -> int:
    print(f"\n[INFO] Processing: {image_path}")
    products = extractor.extract(str(image_path))

    if not products:
        print("  [WARN] No products extracted.")
        return 0

    print(f"  [INFO] Extracted {len(products)} product(s).")

    if dry_run:
        print(json.dumps(products, ensure_ascii=False, indent=2))
        return len(products)

    inserted = db_writer.insert_products(products, store_name, bring_date)
    print(f"  [INFO] Inserted {inserted} row(s) into products table.")
    return inserted


def main() -> None:
    parser = argparse.ArgumentParser(
        description="Extract products from catalog images and store in PostgreSQL."
    )

    source_group = parser.add_mutually_exclusive_group(required=True)
    source_group.add_argument("--image", help="Path to a single catalog image.")
    source_group.add_argument("--folder", help="Path to a folder of catalog images.")

    parser.add_argument(
        "--date",
        required=True,
        help="Product bring date in YYYY-MM-DD format.",
    )
    parser.add_argument(
        "--store",
        required=True,
        help="Store name (e.g. BİM, A101, Migros).",
    )
    parser.add_argument(
        "--method",
        required=True,
        choices=["claude", "easyocr", "tesseract"],
        help="Extraction method to use.",
    )
    parser.add_argument(
        "--dry-run",
        action="store_true",
        help="Print extracted products to stdout instead of writing to DB.",
    )

    args = parser.parse_args()

    bring_date = _parse_date(args.date)
    extractor = _build_extractor(args.method)
    images = _collect_images(args)

    total_inserted = 0
    for image_path in images:
        total_inserted += _process_image(
            extractor, image_path, args.store, bring_date, args.dry_run
        )

    action = "extracted (dry-run)" if args.dry_run else "inserted"
    print(f"\n[DONE] Total products {action}: {total_inserted}")


if __name__ == "__main__":
    main()
