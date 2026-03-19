class ProductResult {
  final int productId;
  final String productName;
  final String category;
  final DateTime? productBringDate;
  final String storeName;

  ProductResult({
    required this.productId,
    required this.productName,
    required this.category,
    this.productBringDate,
    required this.storeName,
  });

  factory ProductResult.fromJson(Map<String, dynamic> json) {
    return ProductResult(
      productId: json['productId'],
      productName: json['productName'],
      category: json['category'],
      productBringDate: json['productBringDate'] != null
          ? DateTime.tryParse(json['productBringDate'])
          : null,
      storeName: json['storeName'],
    );
  }
}
