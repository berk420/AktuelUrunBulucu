"""
Bildirim maili gönderici.

notification_requests tablosundaki kayıtlara karşı ürün listesini eşleştirir,
her e-posta adresine tek bir özet mail gönderir.
"""
import smtplib
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText

import psycopg2

import config

_MATCH_SQL = """
SELECT DISTINCT email, searched_product
FROM notification_requests
WHERE LOWER(searched_product) LIKE LOWER(%s)
   OR LOWER(%s) LIKE LOWER('%%' || searched_product || '%%')
"""


def find_matches(product_names: list[str]) -> dict[str, list[str]]:
    """
    Ürün adlarıyla notification_requests tablosunu eşleştirir.

    Returns:
        { "user@example.com": ["Ürün A", "Ürün B"], ... }
    """
    matches: dict[str, list[str]] = {}

    with psycopg2.connect(config.DB_DSN) as conn:
        with conn.cursor() as cur:
            for name in product_names:
                pattern = f"%{name}%"
                cur.execute(_MATCH_SQL, (pattern, name))
                for email, searched in cur.fetchall():
                    matches.setdefault(email, [])
                    if name not in matches[email]:
                        matches[email].append(name)

    return matches


def send_notifications(product_names: list[str]) -> int:
    """
    Eşleşen abonelere bildirim maili gönderir.

    Returns:
        Gönderilen mail sayısı.
    """
    if not config.SMTP_USERNAME or not config.SMTP_PASSWORD:
        raise ValueError(
            "SMTP_USERNAME ve SMTP_PASSWORD ayarlanmamış. "
            "config.py veya environment variable olarak girin."
        )

    matches = find_matches(product_names)
    if not matches:
        return 0

    sent = 0
    with smtplib.SMTP(config.SMTP_HOST, config.SMTP_PORT) as smtp:
        smtp.ehlo()
        smtp.starttls()
        smtp.login(config.SMTP_USERNAME, config.SMTP_PASSWORD)

        for email, products in matches.items():
            msg = _build_message(email, products)
            smtp.sendmail(config.SMTP_FROM, email, msg.as_string())
            sent += 1

    return sent


def _build_message(to_email: str, products: list[str]) -> MIMEMultipart:
    product_rows = "".join(
        f'<li style="padding:4px 0;">{p}</li>' for p in products
    )

    html = f"""
    <div style="font-family:sans-serif;max-width:520px;margin:0 auto;color:#111827;">
      <h2 style="color:#7c3aed;">Aktüel Ürün Bulucu</h2>
      <p>Merhaba,</p>
      <p>Aradığınız ürün(ler) zincir marketlerde stoklara girmiştir:</p>
      <ul style="background:#f9fafb;border-radius:8px;padding:12px 12px 12px 28px;margin:12px 0;">
        {product_rows}
      </ul>
      <p>Yakın şubeyi bulmak için
        <a href="http://localhost:5173" style="color:#7c3aed;">Aktüel Ürün Bulucu</a>'yu ziyaret edebilirsiniz.
      </p>
      <p style="font-size:12px;color:#9ca3af;margin-top:24px;">
        Bu bildirimi siz talep ettiniz. Artık bildirim almak istemiyorsanız dikkate almayınız.
      </p>
    </div>
    """

    msg = MIMEMultipart("alternative")
    msg["Subject"] = "Aradığınız ürün(ler) stoklara girdi!"
    msg["From"]    = f"{config.SMTP_FROM_NAME} <{config.SMTP_FROM}>"
    msg["To"]      = to_email
    msg.attach(MIMEText(html, "html", "utf-8"))
    return msg
