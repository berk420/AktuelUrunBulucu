import 'dart:convert';
import 'package:http/http.dart' as http;
import '../utils/constants.dart';
import '../models/product_result.dart';

Future<String> getUserIp() async {
  final res = await http.get(Uri.parse(ipifyUrl));
  return jsonDecode(res.body)['ip'] as String;
}

Future<({bool found, List<ProductResult> products})> searchProducts(
    String query, String ip) async {
  final uri = Uri.parse('$apiBaseUrl/api/search')
      .replace(queryParameters: {'query': query, 'ip': ip});
  final res = await http.get(uri);
  if (res.statusCode != 200) throw Exception('API hatası');
  final json = jsonDecode(res.body);
  final found = json['found'] as bool;
  final products = found
      ? (json['matchedProducts'] as List)
          .map((e) => ProductResult.fromJson(e))
          .toList()
      : <ProductResult>[];
  return (found: found, products: products);
}

Future<void> saveUserLocation(String ip, double lat, double lon) async {
  await http.post(
    Uri.parse('$apiBaseUrl/api/location'),
    headers: {'Content-Type': 'application/json'},
    body: jsonEncode({'ip': ip, 'latitude': lat, 'longitude': lon}),
  );
}

Future<void> subscribeNotification(
    String ip, String email, String product) async {
  final res = await http.post(
    Uri.parse('$apiBaseUrl/api/notify'),
    headers: {'Content-Type': 'application/json'},
    body: jsonEncode({'ip': ip, 'email': email, 'product': product}),
  );
  if (res.statusCode != 200) throw Exception('Bildirim kaydedilemedi');
}
