import 'package:flutter/material.dart';
import '../api/recommendation_api.dart';
import '../models/product_result.dart';
import '../utils/constants.dart';

const _staleDays = 7;

bool _isStale(DateTime? date) {
  if (date == null) return false;
  return DateTime.now().difference(date).inDays > _staleDays;
}

class DiscountedScreen extends StatefulWidget {
  const DiscountedScreen({super.key});

  @override
  State<DiscountedScreen> createState() => _DiscountedScreenState();
}

class _DiscountedScreenState extends State<DiscountedScreen> {
  List<({String store, List<ProductResult> products})> _groups = [];
  String? _activeStore;
  bool _loading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    try {
      final products = await getDiscountedProducts();
      final map = <String, List<ProductResult>>{};
      for (final p in products) {
        map.putIfAbsent(p.storeName, () => []).add(p);
      }
      final groups = map.entries
          .map((e) => (store: e.key, products: e.value))
          .toList()
        ..sort((a, b) => b.products.length.compareTo(a.products.length));
      setState(() {
        _groups = groups;
        _activeStore = groups.isNotEmpty ? groups.first.store : null;
        _loading = false;
      });
    } catch (_) {
      setState(() {
        _error = 'Ürünler yüklenirken bir hata oluştu.';
        _loading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    if (_loading) {
      return const Scaffold(
        backgroundColor: Color(0xFFF9FAFB),
        body: Center(
          child: Column(mainAxisSize: MainAxisSize.min, children: [
            CircularProgressIndicator(),
            SizedBox(height: 12),
            Text('Kampanyalı ürünler yükleniyor...',
                style: TextStyle(fontSize: 14, color: Color(0xFF6B7280))),
          ]),
        ),
      );
    }

    if (_error != null) {
      return Scaffold(
        backgroundColor: const Color(0xFFF9FAFB),
        body: Center(
          child: Text(_error!, style: const TextStyle(color: Color(0xFFEF4444), fontSize: 14)),
        ),
      );
    }

    final totalProducts = _groups.fold(0, (s, g) => s + g.products.length);
    final activeGroup = _groups.where((g) => g.store == _activeStore).firstOrNull;

    return Scaffold(
      backgroundColor: const Color(0xFFF9FAFB),
      body: SafeArea(
        child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
          // Başlık
          Padding(
            padding: const EdgeInsets.fromLTRB(16, 16, 16, 0),
            child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
              const Text('Piyasa Fiyatının Altında Ürünler',
                  style: TextStyle(fontSize: 18, fontWeight: FontWeight.w800, color: Color(0xFF111827))),
              const SizedBox(height: 4),
              Text(
                'Aktüel ürünler normal piyasa fiyatından daha uygun. ($totalProducts ürün)',
                style: const TextStyle(fontSize: 13, color: Color(0xFF6B7280)),
              ),
              const SizedBox(height: 16),
            ]),
          ),

          // Market sekmeleri
          Container(
            margin: const EdgeInsets.symmetric(horizontal: 16),
            padding: const EdgeInsets.all(10),
            decoration: BoxDecoration(
              color: const Color(0xFFF3F4F6),
              borderRadius: BorderRadius.circular(12),
            ),
            child: SingleChildScrollView(
              scrollDirection: Axis.horizontal,
              child: Row(
                children: _groups.map((g) {
                  final color = Color(chainColors[g.store] ?? 0xFF6B7280);
                  final active = g.store == _activeStore;
                  return Padding(
                    padding: const EdgeInsets.only(right: 8),
                    child: GestureDetector(
                      onTap: () => setState(() => _activeStore = g.store),
                      child: AnimatedContainer(
                        duration: const Duration(milliseconds: 150),
                        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 8),
                        decoration: BoxDecoration(
                          color: active ? color : Colors.white,
                          border: Border.all(color: active ? color : const Color(0xFFE5E7EB), width: 2),
                          borderRadius: BorderRadius.circular(999),
                        ),
                        child: Row(mainAxisSize: MainAxisSize.min, children: [
                          Text(g.store,
                              style: TextStyle(
                                fontSize: 13,
                                fontWeight: active ? FontWeight.w700 : FontWeight.normal,
                                color: active ? Colors.white : const Color(0xFF374151),
                              )),
                          const SizedBox(width: 6),
                          Container(
                            padding: const EdgeInsets.symmetric(horizontal: 6, vertical: 1),
                            decoration: BoxDecoration(
                              color: active ? Colors.white30 : const Color(0xFFF3F4F6),
                              borderRadius: BorderRadius.circular(999),
                            ),
                            child: Text(
                              '${g.products.length}',
                              style: TextStyle(
                                fontSize: 11,
                                fontWeight: FontWeight.w700,
                                color: active ? Colors.white : const Color(0xFF6B7280),
                              ),
                            ),
                          ),
                        ]),
                      ),
                    ),
                  );
                }).toList(),
              ),
            ),
          ),
          const SizedBox(height: 16),

          // Ürün listesi
          if (activeGroup != null)
            Expanded(
              child: ListView.builder(
                padding: const EdgeInsets.fromLTRB(16, 0, 16, 16),
                itemCount: activeGroup.products.length + 1,
                itemBuilder: (_, i) {
                  if (i == 0) {
                    return Padding(
                      padding: const EdgeInsets.only(bottom: 12),
                      child: Text(
                        '${activeGroup.store} — ${activeGroup.products.length} kampanyalı ürün',
                        style: const TextStyle(
                            fontSize: 13, fontWeight: FontWeight.w600, color: Color(0xFF374151)),
                      ),
                    );
                  }
                  return _ProductCard(product: activeGroup.products[i - 1]);
                },
              ),
            ),
        ]),
      ),
    );
  }
}

class _ProductCard extends StatelessWidget {
  final ProductResult product;
  const _ProductCard({required this.product});

  Color get _color {
    final c = chainColors[product.storeName];
    return c != null ? Color(c) : Colors.grey;
  }

  @override
  Widget build(BuildContext context) {
    final stale = _isStale(product.productBringDate);
    return Container(
      margin: const EdgeInsets.only(bottom: 10),
      decoration: BoxDecoration(
        color: Colors.white,
        border: Border(left: BorderSide(color: _color, width: 4)),
        borderRadius: BorderRadius.circular(10),
        boxShadow: [BoxShadow(color: Colors.black.withAlpha(13), blurRadius: 3)],
      ),
      padding: const EdgeInsets.all(14),
      child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
        Row(mainAxisAlignment: MainAxisAlignment.spaceBetween, children: [
          Expanded(
            child: Text(product.productName,
                style: const TextStyle(fontWeight: FontWeight.w600, fontSize: 14, color: Color(0xFF111827))),
          ),
          const SizedBox(width: 8),
          Container(
            padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
            decoration: BoxDecoration(
              color: const Color(0xFFDCFCE7),
              border: Border.all(color: const Color(0xFFBBF7D0)),
              borderRadius: BorderRadius.circular(6),
            ),
            child: const Text('Piyasa Altı Fiyat',
                style: TextStyle(fontSize: 10, fontWeight: FontWeight.w700, color: Color(0xFF166534))),
          ),
        ]),
        const SizedBox(height: 4),
        Text('Kategori: ${product.category}',
            style: const TextStyle(fontSize: 12, color: Color(0xFF9CA3AF))),
        const SizedBox(height: 8),
        Row(mainAxisAlignment: MainAxisAlignment.spaceBetween, children: [
          _StoreBadge(name: product.storeName, color: _color),
          Row(children: [
            if (product.productBringDate != null)
              Text('Geliş: ${_fmt(product.productBringDate!)}',
                  style: const TextStyle(fontSize: 11, color: Color(0xFF9CA3AF))),
            if (stale) ...[
              const SizedBox(width: 6),
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 6, vertical: 1),
                decoration: BoxDecoration(
                  color: const Color(0xFFFFFBEB),
                  border: Border.all(color: const Color(0xFFF59E0B)),
                  borderRadius: BorderRadius.circular(6),
                ),
                child: const Text('⚠ Tükenmiş olabilir',
                    style: TextStyle(fontSize: 10, color: Color(0xFF92400E))),
              ),
            ],
          ]),
        ]),
      ]),
    );
  }

  String _fmt(DateTime d) =>
      '${d.day.toString().padLeft(2, '0')}.${d.month.toString().padLeft(2, '0')}.${d.year}';
}

class _StoreBadge extends StatelessWidget {
  final String name;
  final Color color;
  const _StoreBadge({required this.name, required this.color});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 2),
      decoration: BoxDecoration(color: color, borderRadius: BorderRadius.circular(999)),
      child: Text(name,
          style: const TextStyle(color: Colors.white, fontSize: 11, fontWeight: FontWeight.w700)),
    );
  }
}
