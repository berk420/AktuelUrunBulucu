"""Writes extracted product data to the PostgreSQL products table."""
from datetime import datetime

import psycopg2

import config

_INSERT_SQL = """
INSERT INTO products (name, category, store_name, product_bring_date)
VALUES (%s, %s, %s, %s)
"""


def insert_products(
    products: list[dict],
    store_name: str,
    bring_date: datetime,
) -> int:
    """
    Insert a list of products into the ``products`` table.

    Args:
        products:   List of dicts with ``name`` and ``category`` keys.
        store_name: Market name (e.g. "BİM", "A101", "Migros").
        bring_date: The date the products arrive / are active in the catalog.

    Returns:
        Number of rows inserted.
    """
    if not products:
        return 0

    rows = [
        (p["name"], p["category"], store_name, bring_date)
        for p in products
    ]

    with psycopg2.connect(config.DB_DSN) as conn:
        with conn.cursor() as cur:
            cur.executemany(_INSERT_SQL, rows)
        conn.commit()

    return len(rows)
