"""
Simple Flask web UI for ProductDataProvider.

Run:
    pip install flask
    python server.py

Then open http://localhost:5050
"""
import os
import tempfile
import traceback
from datetime import datetime
from pathlib import Path

from flask import Flask, jsonify, render_template_string, request

import db_writer

app = Flask(__name__)
app.config["MAX_CONTENT_LENGTH"] = 100 * 1024 * 1024  # 100 MB

STORES = ["BİM", "ŞOK", "A101"]

HTML = open(Path(__file__).parent / "ui.html", encoding="utf-8").read()


@app.route("/")
def index():
    return render_template_string(HTML)


@app.route("/extract", methods=["POST"])
def extract():
    """
    Receive uploaded image files, run extraction, return products JSON.
    Does NOT write to DB.

    Form fields per store (key = BIM / SOK / A101):
        file_BIM   — image file upload
        date_BIM   — YYYY-MM-DD

    Query param:
        method — claude | easyocr | tesseract
    """
    method = request.form.get("method", "claude")

    try:
        extractor = _build_extractor(method)
    except Exception as exc:
        return jsonify({"ok": False, "error": str(exc)}), 400

    results = {}

    for store in STORES:
        key = _store_key(store)
        file = request.files.get(f"file_{key}")
        date_str = request.form.get(f"date_{key}", "")

        if not file or file.filename == "":
            results[store] = {"skipped": True, "products": []}
            continue

        suffix = Path(file.filename).suffix.lower() or ".jpg"

        with tempfile.NamedTemporaryFile(suffix=suffix, delete=False) as tmp:
            file.save(tmp.name)
            tmp_path = tmp.name

        try:
            products = extractor.extract(tmp_path)
        except Exception as exc:
            return jsonify({
                "ok": False,
                "error": f"{store} çıkarma hatası: {exc}\n{traceback.format_exc()}"
            }), 500
        finally:
            os.unlink(tmp_path)

        results[store] = {
            "skipped": False,
            "date": date_str,
            "count": len(products),
            "products": products,
        }

    return jsonify({"ok": True, "results": results})


@app.route("/save", methods=["POST"])
def save():
    """
    Write previously-extracted products to DB.

    JSON body:
        {
          "BİM":  { "date": "2026-03-13", "products": [...] },
          "ŞOK":  { "date": "2026-04-01", "products": [...] },
          "A101": { "date": "2026-03-13", "products": [...] }
        }
    """
    payload: dict = request.get_json(force=True)
    saved = {}

    for store, data in payload.items():
        products = data.get("products", [])
        date_str = data.get("date", "")

        if not products or not date_str:
            saved[store] = 0
            continue

        try:
            bring_date = datetime.strptime(date_str, "%Y-%m-%d")
        except ValueError:
            return jsonify({"ok": False, "error": f"Geçersiz tarih: {date_str}"}), 400

        count = db_writer.insert_products(products, store, bring_date)
        saved[store] = count

    return jsonify({"ok": True, "saved": saved})


# ------------------------------------------------------------------

def _store_key(store: str) -> str:
    return store.replace("İ", "I").replace("Ş", "S").replace("Ö", "O")


def _build_extractor(method: str):
    if method == "claude":
        from extractor.claude_extractor import ClaudeExtractor
        return ClaudeExtractor()
    if method == "easyocr":
        from extractor.easyocr_extractor import EasyOcrExtractor
        return EasyOcrExtractor()
    if method == "tesseract":
        from extractor.tesseract_extractor import TesseractExtractor
        return TesseractExtractor()
    raise ValueError(f"Bilinmeyen method: {method!r}")


if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5050, debug=True)
