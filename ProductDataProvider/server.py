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
        uploaded_files = request.files.getlist(f"file_{key}")
        uploaded_files = [f for f in uploaded_files if f.filename != ""]
        date_str = request.form.get(f"date_{key}", "")

        if not uploaded_files:
            results[store] = {"skipped": True, "products": []}
            continue

        all_products: list[dict] = []

        for file in uploaded_files:
            suffix = Path(file.filename).suffix.lower() or ".jpg"

            with tempfile.NamedTemporaryFile(suffix=suffix, delete=False) as tmp:
                file.save(tmp.name)
                tmp_path = tmp.name

            try:
                products = extractor.extract(tmp_path)
                all_products.extend(products)
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
            "count": len(all_products),
            "products": all_products,
        }

    return jsonify({"ok": True, "results": results})


@app.route("/manual-add", methods=["POST"])
def manual_add():
    """
    Tek bir ürünü manuel olarak DB'ye ekler.

    JSON body:
        { "name": "...", "category": "...", "store": "...", "date": "YYYY-MM-DD" }
    """
    data = request.get_json(force=True)
    name     = (data.get("name")     or "").strip()
    category = (data.get("category") or "").strip()
    store    = (data.get("store")    or "").strip()
    date_str = (data.get("date")     or "").strip()

    if not name or not store or not date_str:
        return jsonify({"ok": False, "error": "Ürün adı, market ve tarih zorunludur."}), 400

    try:
        bring_date = datetime.strptime(date_str, "%Y-%m-%d")
    except ValueError:
        return jsonify({"ok": False, "error": f"Geçersiz tarih: {date_str}"}), 400

    db_writer.insert_products(
        [{"name": name, "category": category or "Diğer"}],
        store,
        bring_date,
    )
    return jsonify({"ok": True})


@app.route("/send-notifications", methods=["POST"])
def send_notifications():
    """
    Kaydedilen ürünlerle eşleşen abonelere bildirim maili gönderir.

    JSON body:
        { "products": ["Ürün A", "Ürün B", ...] }
    """
    import mail_sender

    data = request.get_json(force=True)
    product_names = data.get("products", [])

    if not product_names:
        return jsonify({"ok": False, "error": "Ürün listesi boş."}), 400

    try:
        sent = mail_sender.send_notifications(product_names)
    except Exception as exc:
        return jsonify({"ok": False, "error": str(exc)}), 500

    return jsonify({"ok": True, "sent": sent})


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
