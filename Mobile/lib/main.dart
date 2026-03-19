import 'package:flutter/material.dart';
import 'screens/home_screen.dart';

void main() {
  runApp(const AktuelApp());
}

class AktuelApp extends StatelessWidget {
  const AktuelApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Aktüel Ürün Bulucu',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: const Color(0xFF1976D2)),
        useMaterial3: true,
      ),
      home: const HomeScreen(),
    );
  }
}
