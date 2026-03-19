import 'package:flutter/material.dart';
import '../models/product_result.dart';
import '../utils/constants.dart';

class StoreCard extends StatelessWidget {
  final ProductResult product;

  const StoreCard({super.key, required this.product});

  Color get chainColor {
    final c = chainColors[product.storeName];
    return c != null ? Color(c) : Colors.grey;
  }

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(10),
        side: BorderSide(color: chainColor, width: 0),
      ),
      child: Container(
        decoration: BoxDecoration(
          border: Border(left: BorderSide(color: chainColor, width: 4)),
          borderRadius: BorderRadius.circular(10),
        ),
        padding: const EdgeInsets.all(12),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(product.productName,
                style: const TextStyle(fontWeight: FontWeight.w600, fontSize: 14)),
            const SizedBox(height: 4),
            Text('Kategori: ${product.category}',
                style: const TextStyle(fontSize: 12, color: Colors.grey)),
            if (product.productBringDate != null) ...[
              const SizedBox(height: 2),
              Text(
                'Geliş: ${_formatDate(product.productBringDate!)}',
                style: const TextStyle(fontSize: 12, color: Colors.grey),
              ),
            ],
            const SizedBox(height: 8),
            Container(
              padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 3),
              decoration: BoxDecoration(
                color: chainColor,
                borderRadius: BorderRadius.circular(999),
              ),
              child: Text(
                product.storeName,
                style: const TextStyle(
                    color: Colors.white,
                    fontSize: 11,
                    fontWeight: FontWeight.w600),
              ),
            ),
          ],
        ),
      ),
    );
  }

  String _formatDate(DateTime d) =>
      '${d.day.toString().padLeft(2, '0')}.${d.month.toString().padLeft(2, '0')}.${d.year}';
}
