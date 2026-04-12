import 'dart:convert';
import 'package:http/http.dart' as http;
import '../utils/constants.dart';
import '../models/product_result.dart';
import '../models/recommendation_group.dart';

/// İlgi alanlarına göre ürün önerileri getirir.
Future<List<RecommendationGroup>> getRecommendations(
    List<String> interests) async {
  if (interests.isEmpty) return [];
  final params =
      interests.map((i) => 'interests=${Uri.encodeComponent(i)}').join('&');
  final res =
      await http.get(Uri.parse('$apiBaseUrl/api/recommendations?$params'));
  if (res.statusCode != 200) throw Exception('API hatası');
  final json = jsonDecode(res.body);
  return (json['results'] as List)
      .map((e) => RecommendationGroup.fromJson(e))
      .toList();
}

/// Tüm aktüel ürünleri getirir.
Future<List<ProductResult>> getDiscountedProducts() async {
  final res = await http.get(Uri.parse('$apiBaseUrl/api/products'));
  if (res.statusCode != 200) throw Exception('API hatası');
  final list = jsonDecode(res.body) as List;
  return list.map((e) => ProductResult.fromJson(e)).toList();
}
