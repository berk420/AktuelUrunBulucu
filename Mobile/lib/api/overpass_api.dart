import 'dart:convert';
import 'package:http/http.dart' as http;
import '../utils/constants.dart';
import '../models/osm_store.dart';

const String _bbox = '39.75,32.55,40.15,33.15';

Future<List<OsmStore>> fetchOsmStores() async {
  final query = '''
    [out:json][timeout:30];
    (
      node["shop"="supermarket"]($_bbox);
      node["shop"="convenience"]["name"~"Migros|A101|BİM|ŞOK|Şok|CarrefourSA",i]($_bbox);
    );
    out body;
  ''';

  final res = await http.post(
    Uri.parse(overpassUrl),
    headers: {'Content-Type': 'application/x-www-form-urlencoded'},
    body: 'data=${Uri.encodeComponent(query)}',
  );

  if (res.statusCode != 200) throw Exception('Overpass API hatası');

  final data = jsonDecode(res.body);
  return (data['elements'] as List).map((el) {
    final tags = el['tags'] as Map<String, dynamic>? ?? {};
    return OsmStore(
      id: el['id'],
      name: tags['name'] ?? tags['operator'] ?? 'Market',
      chain: _detectChain(tags['name'], tags['operator'], tags['brand']),
      lat: (el['lat'] as num).toDouble(),
      lon: (el['lon'] as num).toDouble(),
    );
  }).toList();
}

String _detectChain(String? name, String? operator_, String? brand) {
  final text = '${name ?? ''} ${operator_ ?? ''} ${brand ?? ''}'.toLowerCase();
  if (text.contains('migros')) return 'Migros';
  if (text.contains('a101')) return 'A101';
  if (text.contains('bim') || text.contains('bİm')) return 'BİM';
  if (text.contains('şok') || text.contains('sok')) return 'Şok';
  if (text.contains('carrefour')) return 'CarrefourSA';
  return '';
}
