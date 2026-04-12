import 'product_result.dart';

class RecommendationGroup {
  final String interest;
  final List<ProductResult> products;

  RecommendationGroup({required this.interest, required this.products});

  factory RecommendationGroup.fromJson(Map<String, dynamic> json) {
    return RecommendationGroup(
      interest: json['interest'] as String,
      products: (json['products'] as List)
          .map((e) => ProductResult.fromJson(e))
          .toList(),
    );
  }
}
