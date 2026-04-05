const String apiBaseUrl = String.fromEnvironment(
  'API_BASE_URL',
  defaultValue: 'http://10.0.2.2:5012',
);
const String overpassUrl = 'https://overpass-api.de/api/interpreter';
const String ipifyUrl = 'https://api.ipify.org?format=json';
const double maxRadiusKm = 10.0;

const List<Map<String, dynamic>> distanceRings = [
  {'radius': 1000.0, 'label': '1 km', 'color': 0xFF22c55e},
  {'radius': 3000.0, 'label': '3 km', 'color': 0xFFf59e0b},
  {'radius': 5000.0, 'label': '5 km', 'color': 0xFFef4444},
];

const Map<String, int> chainColors = {
  'Migros': 0xFFf97316,
  'A101': 0xFFef4444,
  'BİM': 0xFF3b82f6,
  'Şok': 0xFF8b5cf6,
  'CarrefourSA': 0xFF10b981,
};
