import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../api/recommendation_api.dart';
import '../models/recommendation_group.dart';
import '../models/product_result.dart';
import '../utils/constants.dart';

const _storageKey = 'aktuel_interests';
const _staleDays = 7;

const _suggested = [
  'Spor', 'Kamp', 'Elektronik', 'Bahçe', 'Mobilya',
  'Giyim', 'Çocuk', 'Beyaz Eşya', 'Yaz', 'Bisiklet',
];

bool _isStale(DateTime? date) {
  if (date == null) return false;
  return DateTime.now().difference(date).inDays > _staleDays;
}

class InterestsScreen extends StatefulWidget {
  const InterestsScreen({super.key});

  @override
  State<InterestsScreen> createState() => _InterestsScreenState();
}

class _InterestsScreenState extends State<InterestsScreen> {
  final _inputController = TextEditingController();
  List<String> _interests = [];
  List<RecommendationGroup> _results = [];
  bool _loading = false;

  @override
  void initState() {
    super.initState();
    _loadInterests();
  }

  Future<void> _loadInterests() async {
    final prefs = await SharedPreferences.getInstance();
    final saved = prefs.getStringList(_storageKey) ?? [];
    setState(() => _interests = saved);
    if (saved.isNotEmpty) _fetchRecs(saved);
  }

  Future<void> _saveInterests(List<String> updated) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setStringList(_storageKey, updated);
    setState(() => _interests = updated);
    _fetchRecs(updated);
  }

  Future<void> _fetchRecs(List<String> interests) async {
    if (interests.isEmpty) {
      setState(() => _results = []);
      return;
    }
    setState(() => _loading = true);
    try {
      final data = await getRecommendations(interests);
      setState(() => _results = data);
    } catch (_) {
      setState(() => _results = []);
    } finally {
      setState(() => _loading = false);
    }
  }

  void _addInterest() {
    final v = _inputController.text.trim();
    if (v.isEmpty || _interests.contains(v)) {
      _inputController.clear();
      return;
    }
    _saveInterests([..._interests, v]);
    _inputController.clear();
  }

  void _removeInterest(String item) =>
      _saveInterests(_interests.where((i) => i != item).toList());

  void _addSuggested(String s) {
    if (!_interests.contains(s)) _saveInterests([..._interests, s]);
  }

  @override
  Widget build(BuildContext context) {
    final totalProducts = _results.fold(0, (s, r) => s + r.products.length);

    return Scaffold(
      backgroundColor: const Color(0xFFF9FAFB),
      body: SafeArea(
        child: ListView(
          padding: const EdgeInsets.all(16),
          children: [
            const Text('İlgi Alanlarım',
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.w800, color: Color(0xFF111827))),
            const SizedBox(height: 4),
            const Text('Hobinizi yazın — eşleşen aktüel ürünler otomatik listelenir.',
                style: TextStyle(fontSize: 13, color: Color(0xFF6B7280))),
            const SizedBox(height: 20),

            // Input
            Row(children: [
              Expanded(
                child: TextField(
                  controller: _inputController,
                  onSubmitted: (_) => _addInterest(),
                  textInputAction: TextInputAction.done,
                  decoration: InputDecoration(
                    hintText: 'Örn: Kamp, Bisiklet...',
                    filled: true,
                    fillColor: Colors.white,
                    contentPadding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(8),
                      borderSide: const BorderSide(color: Color(0xFFD1D5DB)),
                    ),
                    enabledBorder: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(8),
                      borderSide: const BorderSide(color: Color(0xFFD1D5DB)),
                    ),
                  ),
                ),
              ),
              const SizedBox(width: 8),
              ElevatedButton(
                onPressed: _addInterest,
                style: ElevatedButton.styleFrom(
                  backgroundColor: const Color(0xFF374151),
                  foregroundColor: Colors.white,
                  padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 12),
                  shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                ),
                child: const Text('Ekle', style: TextStyle(fontWeight: FontWeight.w600)),
              ),
            ]),
            const SizedBox(height: 14),

            // Seçilen ilgi alanları
            if (_interests.isNotEmpty) ...[
              const Text('SEÇİLEN İLGİ ALANLARI',
                  style: TextStyle(fontSize: 11, fontWeight: FontWeight.w600,
                      color: Color(0xFF6B7280), letterSpacing: 0.5)),
              const SizedBox(height: 8),
              Wrap(
                spacing: 8,
                runSpacing: 8,
                children: _interests.map((item) => _Chip(
                  label: item,
                  selected: true,
                  onRemove: () => _removeInterest(item),
                )).toList(),
              ),
              const SizedBox(height: 16),
            ],

            // Hızlı ekle
            const Text('HIZLI EKLE',
                style: TextStyle(fontSize: 11, fontWeight: FontWeight.w600,
                    color: Color(0xFF6B7280), letterSpacing: 0.5)),
            const SizedBox(height: 8),
            Wrap(
              spacing: 6,
              runSpacing: 6,
              children: _suggested.map((s) {
                final added = _interests.contains(s);
                return GestureDetector(
                  onTap: () => !added ? _addSuggested(s) : null,
                  child: Container(
                    padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
                    decoration: BoxDecoration(
                      color: added ? const Color(0xFFE5E7EB) : const Color(0xFFF3F4F6),
                      border: Border.all(
                          color: added ? const Color(0xFFD1D5DB) : const Color(0xFFE5E7EB)),
                      borderRadius: BorderRadius.circular(999),
                    ),
                    child: Text(
                      '${added ? '✓ ' : '+ '}$s',
                      style: TextStyle(
                        fontSize: 12,
                        color: added ? const Color(0xFF9CA3AF) : const Color(0xFF374151),
                      ),
                    ),
                  ),
                );
              }).toList(),
            ),
            const SizedBox(height: 28),

            // Sonuçlar
            if (_interests.isNotEmpty) ...[
              Row(children: [
                const Text('Hobinize Özel Aktüel Ürünler',
                    style: TextStyle(fontSize: 16, fontWeight: FontWeight.w700, color: Color(0xFF111827))),
                const SizedBox(width: 12),
                if (!_loading)
                  Text('$totalProducts ürün',
                      style: const TextStyle(fontSize: 12, color: Color(0xFF6B7280))),
              ]),
              const SizedBox(height: 16),

              if (_loading)
                const Center(child: CircularProgressIndicator())
              else
                ..._results.map((group) => _InterestGroup(group: group)),
            ] else
              Container(
                padding: const EdgeInsets.all(40),
                decoration: BoxDecoration(
                  border: Border.all(color: const Color(0xFFE5E7EB), width: 2),
                  borderRadius: BorderRadius.circular(12),
                ),
                child: const Center(
                  child: Text(
                    'Yukarıdan ilgi alanı ekleyin,\neşleşen aktüel ürünler burada görünecek.',
                    textAlign: TextAlign.center,
                    style: TextStyle(fontSize: 14, color: Color(0xFF9CA3AF)),
                  ),
                ),
              ),
          ],
        ),
      ),
    );
  }

  @override
  void dispose() {
    _inputController.dispose();
    super.dispose();
  }
}

class _Chip extends StatelessWidget {
  final String label;
  final bool selected;
  final VoidCallback? onRemove;
  const _Chip({required this.label, required this.selected, this.onRemove});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 5),
      decoration: BoxDecoration(
        color: const Color(0xFF374151),
        borderRadius: BorderRadius.circular(999),
      ),
      child: Row(mainAxisSize: MainAxisSize.min, children: [
        Text(label, style: const TextStyle(color: Colors.white, fontSize: 13, fontWeight: FontWeight.w500)),
        if (onRemove != null) ...[
          const SizedBox(width: 6),
          GestureDetector(
            onTap: onRemove,
            child: Container(
              width: 16, height: 16,
              decoration: BoxDecoration(
                color: Colors.white24,
                borderRadius: BorderRadius.circular(8),
              ),
              child: const Icon(Icons.close, size: 10, color: Colors.white),
            ),
          ),
        ],
      ]),
    );
  }
}

class _InterestGroup extends StatelessWidget {
  final RecommendationGroup group;
  const _InterestGroup({required this.group});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 28),
      child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
        Row(children: [
          Container(
            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 3),
            decoration: BoxDecoration(
              color: const Color(0xFF374151),
              borderRadius: BorderRadius.circular(999),
            ),
            child: Text(group.interest,
                style: const TextStyle(color: Colors.white, fontSize: 12, fontWeight: FontWeight.w700)),
          ),
          const SizedBox(width: 10),
          Text(
            group.products.isEmpty ? 'eşleşen ürün bulunamadı' : '${group.products.length} ürün',
            style: const TextStyle(fontSize: 12, color: Color(0xFF9CA3AF)),
          ),
        ]),
        const SizedBox(height: 12),
        if (group.products.isEmpty)
          Container(
            width: double.infinity,
            padding: const EdgeInsets.all(14),
            decoration: BoxDecoration(
              color: const Color(0xFFF9FAFB),
              border: Border.all(color: const Color(0xFFD1D5DB), style: BorderStyle.solid),
              borderRadius: BorderRadius.circular(8),
            ),
            child: const Text('Bu ilgi alanına ait aktüel ürün bulunamadı.',
                textAlign: TextAlign.center,
                style: TextStyle(fontSize: 13, color: Color(0xFF9CA3AF))),
          )
        else
          ...group.products.map((p) => _ProductCard(product: p, interest: group.interest)),
      ]),
    );
  }
}

class _ProductCard extends StatelessWidget {
  final ProductResult product;
  final String interest;
  const _ProductCard({required this.product, required this.interest});

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
        Text(product.productName,
            style: const TextStyle(fontWeight: FontWeight.w600, fontSize: 14, color: Color(0xFF111827))),
        const SizedBox(height: 3),
        Text('Kategori: ${product.category}',
            style: const TextStyle(fontSize: 12, color: Color(0xFF9CA3AF))),
        const SizedBox(height: 8),
        Row(mainAxisAlignment: MainAxisAlignment.spaceBetween, children: [
          _StoreBadge(name: product.storeName, color: _color),
          Row(children: [
            if (product.productBringDate != null)
              Text(
                'Geliş: ${_fmt(product.productBringDate!)}',
                style: const TextStyle(fontSize: 11, color: Color(0xFF9CA3AF)),
              ),
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
        const SizedBox(height: 10),
        Container(
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 7),
          decoration: BoxDecoration(
            color: const Color(0xFFF0FDF4),
            border: Border.all(color: const Color(0xFFBBF7D0)),
            borderRadius: BorderRadius.circular(8),
          ),
          child: RichText(
            text: TextSpan(
              style: const TextStyle(fontSize: 12, color: Color(0xFF166534)),
              children: [
                TextSpan(text: interest, style: const TextStyle(fontWeight: FontWeight.bold)),
                const TextSpan(text: ' hobinize ait bu aktüel ürün '),
                TextSpan(text: product.storeName, style: const TextStyle(fontWeight: FontWeight.bold)),
                const TextSpan(text: ' marketinde bulunmaktadır.'),
              ],
            ),
          ),
        ),
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
