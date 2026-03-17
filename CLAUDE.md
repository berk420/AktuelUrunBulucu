# Proje Kuralları

## Veritabanı

- Tablo isimleri snake_case olacak (stores, search_logs, product_items)
- Her tablonun PK'sı `id` adında olacak, DB tarafında otomatik artacak (SERIAL / IDENTITY), kod tarafında manuel ID atama yapılmayacak
- İlişkilerde FK kolonları entity içinde açıkça tanımlanacak, EF shadow property kullanılmayacak
  - Doğru: `public int StoreId { get; set; }` + `public Store Store { get; set; }`
  - Yanlış: sadece `public Store Store { get; set; }` bırakmak

## Kod Yazım

- Her public metod XML summary içerecek (`/// <summary>`)
- Interface'ler `I` prefix ile başlayacak (`IProductRepository`, `ISearchService`)
- Her sınıf için interface tanımlanacak, bağımlılıklar interface üzerinden inject edilecek
- SOLID prensipleri uygulanacak:
  - Single Responsibility: her sınıfın tek sorumluluğu olacak
  - Open/Closed: genişlemeye açık, değişime kapalı
  - Liskov Substitution: türetilmiş sınıflar base sınıfın yerine geçebilmeli
  - Interface Segregation: küçük, odaklı interface'ler kullanılacak
  - Dependency Inversion: somut sınıfa değil interface'e bağımlı olunacak

## Genel

- Magic number kullanılmayacak, sabitler `const` veya `enum` ile tanımlanacak
  - Yanlış: `if (radius > 1000)`
  - Doğru: `const int MaxRadiusMetres = 1000;`
