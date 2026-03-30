import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_mobile_ads/google_mobile_ads.dart';
import 'package:latlong2/latlong.dart';

import '../api/overpass_api.dart';
import '../api/search_api.dart';
import '../models/osm_store.dart';
import '../models/product_result.dart';
import '../utils/constants.dart';
import '../utils/distance.dart';
import '../widgets/store_card.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  final MapController _mapController = MapController();
  final TextEditingController _searchController = TextEditingController();
  final TextEditingController _emailController = TextEditingController();

  List<OsmStore> _osmStores = [];
  List<ProductResult> _products = [];
  LatLng? _userLatLng;
  String _userIp = '';
  bool _searchPerformed = false;
  bool _notFound = false;
  bool _osmLoading = true;
  bool _searching = false;
  bool _notifSubmitted = false;
  String _lastQuery = '';

  InterstitialAd? _interstitialAd;
  int _searchCount = 0;

  // Test ID — canlıya alırken AdMob'dan aldığın gerçek ID ile değiştir
  static const String _adUnitId = 'ca-app-pub-5451625013655025/3217423027';

  static const LatLng _defaultCenter = LatLng(39.9334, 32.8597);

  @override
  void initState() {
    super.initState();
    _init();
    _loadInterstitialAd(showOnLoad: true);
  }

  void _loadInterstitialAd({bool showOnLoad = false}) {
    InterstitialAd.load(
      adUnitId: _adUnitId,
      request: const AdRequest(),
      adLoadCallback: InterstitialAdLoadCallback(
        onAdLoaded: (ad) {
          _interstitialAd = ad;
          if (showOnLoad) {
            _showAd();
          }
        },
        onAdFailedToLoad: (_) => _interstitialAd = null,
      ),
    );
  }

  void _showAd() {
    if (_interstitialAd == null) return;
    _interstitialAd!.fullScreenContentCallback = FullScreenContentCallback(
      onAdDismissedFullScreenContent: (ad) {
        ad.dispose();
        _loadInterstitialAd();
      },
      onAdFailedToShowFullScreenContent: (ad, _) {
        ad.dispose();
        _loadInterstitialAd();
      },
    );
    _interstitialAd!.show();
    _interstitialAd = null;
  }

  void _showAdIfNeeded() {
    _searchCount++;
    if (_searchCount % 2 == 0) {
      _showAd();
    }
  }

  Future<void> _init() async {
    _userIp = await getUserIp().catchError((_) => '');
    await _loadOsmStores();
    await _requestLocation();
  }

  Future<void> _loadOsmStores() async {
    try {
      final stores = await fetchOsmStores();
      setState(() {
        _osmStores = stores;
        _osmLoading = false;
      });
    } catch (_) {
      setState(() => _osmLoading = false);
    }
  }

  Future<void> _requestLocation() async {
    try {
      LocationPermission perm = await Geolocator.checkPermission();
      if (perm == LocationPermission.denied) {
        perm = await Geolocator.requestPermission();
      }
      if (perm == LocationPermission.deniedForever) return;

      final pos = await Geolocator.getCurrentPosition(
        locationSettings: const LocationSettings(accuracy: LocationAccuracy.high),
      );
      final ll = LatLng(pos.latitude, pos.longitude);
      setState(() => _userLatLng = ll);
      _mapController.move(ll, 13);
      if (_userIp.isNotEmpty) {
        await saveUserLocation(_userIp, pos.latitude, pos.longitude);
      }
    } catch (_) {}
  }

  List<OsmStore> get _nearbyStores {
    if (_userLatLng == null) return _osmStores;
    return _osmStores.where((s) {
      final d = haversineKm(_userLatLng!.latitude, _userLatLng!.longitude, s.lat, s.lon);
      return d <= maxRadiusKm;
    }).toList();
  }

  Set<String> get _matchedChains =>
      _products.map((p) => p.storeName).toSet();

  bool _isMatchedStore(OsmStore s) =>
      _matchedChains.any((chain) => s.chain.toLowerCase() == chain.toLowerCase());

  Future<void> _search() async {
    final query = _searchController.text.trim();
    if (query.isEmpty) return;
    setState(() {
      _searching = true;
      _notifSubmitted = false;
    });

    try {
      final result = await searchProducts(query, _userIp);
      setState(() {
        _lastQuery = query;
        _searchPerformed = true;
        _notFound = !result.found;
        _products = result.products;
        _searching = false;
      });
      _showAdIfNeeded();

      if (result.found && result.products.isNotEmpty) {
        _showResultsSheet();
      } else if (!result.found) {
        _showNotFoundDialog();
      }
    } catch (e) {
      setState(() => _searching = false);
      _showAdIfNeeded();
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Arama hatası: $e')),
        );
      }
    }
  }

  void _showResultsSheet() {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (_) => DraggableScrollableSheet(
        initialChildSize: 0.45,
        minChildSize: 0.15,
        maxChildSize: 0.85,
        builder: (_, controller) => Container(
          decoration: const BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
          ),
          child: Column(
            children: [
              const SizedBox(height: 8),
              Container(
                width: 40,
                height: 4,
                decoration: BoxDecoration(
                  color: Colors.grey[300],
                  borderRadius: BorderRadius.circular(2),
                ),
              ),
              Padding(
                padding: const EdgeInsets.all(12),
                child: Text(
                  '${_products.length} ürün bulundu',
                  style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16),
                ),
              ),
              Expanded(
                child: ListView.builder(
                  controller: controller,
                  itemCount: _products.length,
                  itemBuilder: (_, i) => StoreCard(product: _products[i]),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  void _showNotFoundDialog() {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (_) => Padding(
        padding: EdgeInsets.only(bottom: MediaQuery.of(context).viewInsets.bottom),
        child: Container(
          decoration: const BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
          ),
          padding: const EdgeInsets.all(20),
          child: StatefulBuilder(
            builder: (ctx, setLocal) => Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Container(
                  width: 40,
                  height: 4,
                  decoration: BoxDecoration(
                    color: Colors.grey[300],
                    borderRadius: BorderRadius.circular(2),
                  ),
                ),
                const SizedBox(height: 16),
                const Icon(Icons.search_off, size: 48, color: Colors.orange),
                const SizedBox(height: 12),
                Text(
                  '"$_lastQuery" bulunamadı',
                  style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16),
                ),
                const SizedBox(height: 8),
                const Text(
                  'Ürün aktüele girdiğinde sizi haberdar edelim mi?',
                  textAlign: TextAlign.center,
                  style: TextStyle(color: Colors.grey),
                ),
                const SizedBox(height: 16),
                if (_notifSubmitted)
                  const Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Icon(Icons.check_circle, color: Colors.green),
                      SizedBox(width: 8),
                      Text('Bildirim kaydedildi!', style: TextStyle(color: Colors.green)),
                    ],
                  )
                else ...[
                  TextField(
                    controller: _emailController,
                    keyboardType: TextInputType.emailAddress,
                    decoration: const InputDecoration(
                      hintText: 'E-posta adresiniz',
                      border: OutlineInputBorder(),
                      prefixIcon: Icon(Icons.email_outlined),
                    ),
                  ),
                  const SizedBox(height: 12),
                  SizedBox(
                    width: double.infinity,
                    child: ElevatedButton(
                      onPressed: () async {
                        final email = _emailController.text.trim();
                        if (email.isEmpty) return;
                        try {
                          await subscribeNotification(_userIp, email, _lastQuery);
                          setLocal(() {});
                          setState(() => _notifSubmitted = true);
                        } catch (e) {
                          ScaffoldMessenger.of(context).showSnackBar(
                            SnackBar(content: Text('Hata: $e')),
                          );
                        }
                      },
                      child: const Text('Bildir'),
                    ),
                  ),
                ],
                const SizedBox(height: 8),
              ],
            ),
          ),
        ),
      ),
    );
  }

  List<CircleMarker> get _distanceRings {
    if (!_searchPerformed || _userLatLng == null) return [];
    return distanceRings.map((ring) {
      final color = Color(ring['color'] as int);
      return CircleMarker(
        point: _userLatLng!,
        radius: (ring['radius'] as double),
        useRadiusInMeter: true,
        color: color.withAlpha(20),
        borderColor: color,
        borderStrokeWidth: 2,
      );
    }).toList();
  }

  @override
  Widget build(BuildContext context) {
    final center = _userLatLng ?? _defaultCenter;

    return Scaffold(
      body: Stack(
        children: [
          FlutterMap(
            mapController: _mapController,
            options: MapOptions(
              initialCenter: center,
              initialZoom: 13,
            ),
            children: [
              TileLayer(
                urlTemplate: 'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
                userAgentPackageName: 'com.example.aktuel',
              ),
              CircleLayer(circles: _distanceRings),
              MarkerLayer(
                markers: [
                  ..._nearbyStores.map((s) {
                    final matched = _isMatchedStore(s);
                    final chain = matched ? s.chain : '';
                    final colorVal = chainColors[chain];
                    final color = colorVal != null ? Color(colorVal) : Colors.grey;
                    return Marker(
                      point: LatLng(s.lat, s.lon),
                      width: 32,
                      height: 32,
                      child: Icon(
                        Icons.store,
                        color: matched ? color : Colors.grey[400],
                        size: matched ? 28 : 20,
                      ),
                    );
                  }),
                  if (_userLatLng != null)
                    Marker(
                      point: _userLatLng!,
                      width: 40,
                      height: 40,
                      child: const Icon(Icons.my_location, color: Colors.amber, size: 32),
                    ),
                ],
              ),
            ],
          ),
          SafeArea(
            child: Padding(
              padding: const EdgeInsets.all(12),
              child: Card(
                elevation: 4,
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                child: Padding(
                  padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                  child: Row(
                    children: [
                      Expanded(
                        child: TextField(
                          controller: _searchController,
                          decoration: const InputDecoration(
                            hintText: 'Ürün ara...',
                            border: InputBorder.none,
                            isDense: true,
                          ),
                          onSubmitted: (_) => _search(),
                          textInputAction: TextInputAction.search,
                        ),
                      ),
                      _searching
                          ? const SizedBox(
                              width: 24,
                              height: 24,
                              child: CircularProgressIndicator(strokeWidth: 2),
                            )
                          : IconButton(
                              icon: const Icon(Icons.search),
                              onPressed: _search,
                              padding: EdgeInsets.zero,
                              constraints: const BoxConstraints(),
                            ),
                    ],
                  ),
                ),
              ),
            ),
          ),
          if (_osmLoading)
            const Center(child: CircularProgressIndicator()),
        ],
      ),
      floatingActionButton: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          if (_products.isNotEmpty)
            FloatingActionButton.small(
              heroTag: 'results',
              onPressed: _showResultsSheet,
              child: const Icon(Icons.list),
            ),
          const SizedBox(height: 8),
          FloatingActionButton.small(
            heroTag: 'location',
            onPressed: () {
              if (_userLatLng != null) {
                _mapController.move(_userLatLng!, 14);
              } else {
                _requestLocation();
              }
            },
            child: const Icon(Icons.my_location),
          ),
        ],
      ),
    );
  }

  @override
  void dispose() {
    _searchController.dispose();
    _emailController.dispose();
    _interstitialAd?.dispose();
    super.dispose();
  }
}
