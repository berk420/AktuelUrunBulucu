using AktuelUrunBulucu.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AktuelUrunBulucu.DAL.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<SearchLog> SearchLogs => Set<SearchLog>();
    public DbSet<UserCoordinate> UserCoordinates => Set<UserCoordinate>();
    public DbSet<NotificationRequest> NotificationRequests => Set<NotificationRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<SearchLog>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<UserCoordinate>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<NotificationRequest>(e =>
        {
            e.HasKey(n => n.Id);
            e.Property(n => n.Id).ValueGeneratedOnAdd();
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1,  Name = "Çamaşır Makinesi Samsung 8kg",       Category = "Beyaz Eşya",           StoreName = "Migros",      ProductBringDate = new DateTime(2026, 3,  1, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 12999m },
            new Product { Id = 2,  Name = "Bulaşık Makinesi Arçelik 5 Program", Category = "Beyaz Eşya",           StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 3,  5, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 9499m },
            new Product { Id = 3,  Name = "Buzdolabı Vestel No-Frost",           Category = "Beyaz Eşya",           StoreName = "Migros",      ProductBringDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 15999m },
            new Product { Id = 4,  Name = "Mikrodalga Fırın 20L",                Category = "Küçük Ev Aleti",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1299m },
            new Product { Id = 5,  Name = "Elektrikli Süpürge Rowenta",          Category = "Küçük Ev Aleti",       StoreName = "A101",        ProductBringDate = new DateTime(2026, 3,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 2499m },
            new Product { Id = 6,  Name = "Hava Fritözü 5L",                     Category = "Küçük Ev Aleti",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 3, 12, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1899m },
            new Product { Id = 7,  Name = "Bisiklet 26 Jant Dağ Bisikleti",      Category = "Spor & Outdoor",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4,  1, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 4999m },
            new Product { Id = 8,  Name = "Bisiklet Çocuk 20 Jant",              Category = "Spor & Outdoor",       StoreName = "Migros",      ProductBringDate = new DateTime(2026, 4,  5, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 2799m },
            new Product { Id = 9,  Name = "Çadır 4 Kişilik Kamp Çadırı",         Category = "Kamp & Outdoor",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4, 10, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 3499m },
            new Product { Id = 10, Name = "Çadır 2 Kişilik Ultra Hafif",          Category = "Kamp & Outdoor",       StoreName = "Migros",      ProductBringDate = new DateTime(2026, 4, 10, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 2199m },
            new Product { Id = 11, Name = "Barbekü Izgara Kömürlü Set",           Category = "Bahçe & Piknik",       StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 899m },
            new Product { Id = 12, Name = "Barbekü Izgara Gazlı Taşınabilir",     Category = "Bahçe & Piknik",       StoreName = "A101",        ProductBringDate = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1499m },
            new Product { Id = 13, Name = "Bahçe Hortumu 25m",                    Category = "Bahçe & Piknik",       StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4,  1, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 349m },
            new Product { Id = 14, Name = "Çim Biçme Makinesi Elektrikli",        Category = "Bahçe & Piknik",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4, 20, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 2999m },
            new Product { Id = 15, Name = "Bahçe Masa Sandalye Seti 4+1",         Category = "Mobilya & Dekorasyon", StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4,  5, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 3799m },
            new Product { Id = 16, Name = "Katlanır Kamp Sandalyesi",             Category = "Kamp & Outdoor",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4,  1, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 299m },
            new Product { Id = 17, Name = "Raf Sistemi Metal 5 Katlı",            Category = "Mobilya & Dekorasyon", StoreName = "A101",        ProductBringDate = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1199m },
            new Product { Id = 18, Name = "Uyku Tulumu -5 Derece",                Category = "Kamp & Outdoor",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4,  1, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 799m },
            new Product { Id = 19, Name = "Yağmurluk Unisex L Beden",             Category = "Giyim",                StoreName = "BİM",         ProductBringDate = new DateTime(2026, 3, 25, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 449m },
            new Product { Id = 20, Name = "Spor Ayakkabı Erkek 42",               Category = "Giyim",                StoreName = "Migros",      ProductBringDate = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 999m },
            new Product { Id = 21, Name = "Akıllı Saat Fitness Tracker",           Category = "Elektronik",           StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 2299m },
            new Product { Id = 22, Name = "Bluetooth Hoparlör Su Geçirmez",        Category = "Elektronik",           StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 899m },
            new Product { Id = 23, Name = "Powerbank 20000mAh",                    Category = "Elektronik",           StoreName = "A101",        ProductBringDate = new DateTime(2026, 3,  5, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 499m },
            new Product { Id = 24, Name = "Çocuk Scooter 3 Tekerlekli",            Category = "Çocuk & Oyuncak",      StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4,  5, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 699m },
            new Product { Id = 25, Name = "Kaydırak Çocuk Bahçe Seti",             Category = "Çocuk & Oyuncak",      StoreName = "A101",        ProductBringDate = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1599m },
            new Product { Id = 26, Name = "Matkap Seti Akülü 18V",                 Category = "Araç Gereç",           StoreName = "Şok",         ProductBringDate = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1799m },
            new Product { Id = 27, Name = "El Arabası Plastik Bahçe",              Category = "Bahçe & Piknik",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4, 10, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 599m },
            new Product { Id = 28, Name = "Merdiven 5 Basamak Alüminyum",          Category = "Araç Gereç",           StoreName = "Migros",      ProductBringDate = new DateTime(2026, 3, 25, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1099m },
            new Product { Id = 29, Name = "Şişme Havuz 300x200cm Aile",            Category = "Yaz & Havuz",          StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 5,  1, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 2499m },
            new Product { Id = 30, Name = "Güneş Şemsiyesi 2m UV Korumalı",        Category = "Yaz & Havuz",          StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 5,  1, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 799m },

            // Mangal ürünleri
            new Product { Id = 31, Name = "Mangal Kömürlü Set Takım Çantalı",       Category = "Bahçe & Piknik",       StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 649m },
            new Product { Id = 32, Name = "Mangal Büyük Boy Aile Mangalı",           Category = "Bahçe & Piknik",       StoreName = "A101",        ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 899m },
            new Product { Id = 33, Name = "Mangal Portatif Katlanır Kamp Mangalı",   Category = "Bahçe & Piknik",       StoreName = "Migros",      ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 499m },
            new Product { Id = 34, Name = "Mangal Gazlı Taşınabilir 2 Gözlü",        Category = "Bahçe & Piknik",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1299m },
            new Product { Id = 35, Name = "Mangal Elektrikli İç Mekan 2000W",        Category = "Bahçe & Piknik",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1099m },
            new Product { Id = 36, Name = "Mangal Kömürü 5kg Doğal Meyve Odunu",     Category = "Bahçe & Piknik",       StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 149m },
            new Product { Id = 37, Name = "Mangal Kömürü 10kg Premium",              Category = "Bahçe & Piknik",       StoreName = "A101",        ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 249m },
            new Product { Id = 38, Name = "Mangal Ateş Tutuşturucu Jel 500ml",       Category = "Bahçe & Piknik",       StoreName = "Migros",      ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 79m },
            new Product { Id = 39, Name = "Mangal Izgara Teli 40x60cm Paslanmaz",    Category = "Bahçe & Piknik",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 199m },
            new Product { Id = 40, Name = "Mangal Maşa Spatula Set 5 Parça",         Category = "Bahçe & Piknik",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 169m },
            new Product { Id = 41, Name = "Mangal Eldiveni Isıya Dayanıklı Çift",    Category = "Bahçe & Piknik",       StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 99m },
            new Product { Id = 42, Name = "Mangal Izgarası Döküm Demir 50cm",        Category = "Bahçe & Piknik",       StoreName = "A101",        ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 349m },
            new Product { Id = 43, Name = "Mangal Barbekü Sis Şişi 12li Set",        Category = "Bahçe & Piknik",       StoreName = "Migros",      ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 129m },
            new Product { Id = 44, Name = "Mangal Alüminyum Folyo Tepsi 5li",        Category = "Bahçe & Piknik",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 59m },
            new Product { Id = 45, Name = "Mangal Çantalı Piknik Seti 20 Parça",     Category = "Bahçe & Piknik",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 399m },

            // Powerbank ürünleri
            new Product { Id = 46, Name = "Powerbank 5000mAh Slim Taşınabilir",      Category = "Elektronik",           StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 199m },
            new Product { Id = 47, Name = "Powerbank 10000mAh Hızlı Şarj 22.5W",    Category = "Elektronik",           StoreName = "A101",        ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 349m },
            new Product { Id = 48, Name = "Powerbank 20000mAh Çift USB Çıkış",       Category = "Elektronik",           StoreName = "Migros",      ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 499m },
            new Product { Id = 49, Name = "Powerbank 20000mAh PD 65W Laptop",        Category = "Elektronik",           StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 899m },
            new Product { Id = 50, Name = "Powerbank 30000mAh Süper Kapasite",       Category = "Elektronik",           StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 699m },
            new Product { Id = 51, Name = "Powerbank Kablosuz Şarjlı 15W MagSafe",  Category = "Elektronik",           StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 749m },
            new Product { Id = 52, Name = "Powerbank Solar Güneş Enerjili 10000mAh", Category = "Elektronik",           StoreName = "A101",        ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 599m },
            new Product { Id = 53, Name = "Powerbank Mini Anahtarlık 1500mAh",       Category = "Elektronik",           StoreName = "Migros",      ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 129m },
            new Product { Id = 54, Name = "Powerbank 10000mAh Led Göstergeli",       Category = "Elektronik",           StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 299m },
            new Product { Id = 55, Name = "Powerbank 25000mAh 4 Portlu Hızlı Şarj", Category = "Elektronik",           StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4,  8, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 799m },

            // Gıda ürünleri
            new Product { Id = 56, Name = "Zeytinyağı Riviera 5L Teneke",            Category = "Gıda",                 StoreName = "BİM",         ProductBringDate = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 849m },
            new Product { Id = 57, Name = "Fındık İç 1kg Giresun",                   Category = "Gıda",                 StoreName = "A101",        ProductBringDate = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 699m },
            new Product { Id = 58, Name = "Bal Süzme Çam Balı 850g",                 Category = "Gıda",                 StoreName = "Şok",         ProductBringDate = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 549m },
            new Product { Id = 59, Name = "Çay Rize Çayı 3kg Karton Kutu",           Category = "Gıda",                 StoreName = "Migros",      ProductBringDate = new DateTime(2026, 6, 22, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 399m },
            new Product { Id = 60, Name = "Türk Kahvesi 500g Öğütülmüş",            Category = "Gıda",                 StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 249m },

            // Kozmetik & Kişisel Bakım
            new Product { Id = 61, Name = "Güneş Kremi SPF50 200ml",                 Category = "Kozmetik",             StoreName = "Migros",      ProductBringDate = new DateTime(2026, 6, 10, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 179m },
            new Product { Id = 62, Name = "Saç Kurutma Makinesi 2200W İyonik",       Category = "Kozmetik",             StoreName = "A101",        ProductBringDate = new DateTime(2026, 6, 12, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1299m },
            new Product { Id = 63, Name = "Epilatör Su Geçirmez Şarjlı",             Category = "Kozmetik",             StoreName = "BİM",         ProductBringDate = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 999m },
            new Product { Id = 64, Name = "Parfüm Erkek EDT 100ml",                  Category = "Kozmetik",             StoreName = "Şok",         ProductBringDate = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 599m },

            // Ev Tekstili
            new Product { Id = 65, Name = "Nevresim Takımı Çift Kişilik Pamuk",      Category = "Ev Tekstili",          StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 799m },
            new Product { Id = 66, Name = "Havlu Seti 6lı Banyo Havlusu",            Category = "Ev Tekstili",          StoreName = "BİM",         ProductBringDate = new DateTime(2026, 6, 22, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 349m },
            new Product { Id = 67, Name = "Yastık Visco Ortopedik Boyun",             Category = "Ev Tekstili",          StoreName = "A101",        ProductBringDate = new DateTime(2026, 6, 25, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 499m },

            // Temizlik
            new Product { Id = 68, Name = "Çamaşır Deterjanı 10kg Toz",              Category = "Temizlik",             StoreName = "Şok",         ProductBringDate = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 449m },
            new Product { Id = 69, Name = "Bulaşık Deterjanı Tablet 72li",            Category = "Temizlik",             StoreName = "Migros",      ProductBringDate = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 299m },
            new Product { Id = 70, Name = "Robot Süpürge Akıllı Lazerli",             Category = "Küçük Ev Aleti",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 8999m },

            // Otomotiv
            new Product { Id = 71, Name = "Araç İçi Telefon Tutucu Manyetik",        Category = "Otomotiv",             StoreName = "A101",        ProductBringDate = new DateTime(2026, 6, 10, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 129m },
            new Product { Id = 72, Name = "Araç Kompresörü 12V Dijital",              Category = "Otomotiv",             StoreName = "BİM",         ProductBringDate = new DateTime(2026, 6, 12, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 599m },
            new Product { Id = 73, Name = "Araç Koltuk Kılıfı Deri Universal",       Category = "Otomotiv",             StoreName = "Şok",         ProductBringDate = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 899m },

            // Kırtasiye & Okul
            new Product { Id = 74, Name = "Tablet Kalem Stylus iPad Uyumlu",          Category = "Elektronik",           StoreName = "Migros",      ProductBringDate = new DateTime(2026, 6, 22, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 1499m },
            new Product { Id = 75, Name = "Kulaklık Bluetooth ANC Gürültü Önleyici", Category = "Elektronik",           StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 6, 25, 0, 0, 0, DateTimeKind.Utc), ProductPrice = 3499m }
        );

        modelBuilder.Entity<SearchLog>().HasData(
            new SearchLog { Id = 1,  IpAddress = "85.102.45.12",   SearchedProduct = "mangal",              SearchedAt = new DateTime(2026, 6, 10, 9, 15, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 2,  IpAddress = "78.160.32.88",   SearchedProduct = "powerbank",           SearchedAt = new DateTime(2026, 6, 10, 10, 30, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 3,  IpAddress = "176.42.115.200", SearchedProduct = "çamaşır makinesi",    SearchedAt = new DateTime(2026, 6, 11, 14, 45, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 4,  IpAddress = "85.102.45.12",   SearchedProduct = "bisiklet",            SearchedAt = new DateTime(2026, 6, 11, 16, 20, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 5,  IpAddress = "31.223.78.55",   SearchedProduct = "hava fritözü",        SearchedAt = new DateTime(2026, 6, 12, 8, 0, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 6,  IpAddress = "95.70.130.44",   SearchedProduct = "çadır",               SearchedAt = new DateTime(2026, 6, 12, 11, 10, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 7,  IpAddress = "78.160.32.88",   SearchedProduct = "bluetooth hoparlör",  SearchedAt = new DateTime(2026, 6, 13, 13, 25, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 8,  IpAddress = "176.42.115.200", SearchedProduct = "buzdolabı",           SearchedAt = new DateTime(2026, 6, 13, 15, 55, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 9,  IpAddress = "212.174.60.33",  SearchedProduct = "güneş kremi",         SearchedAt = new DateTime(2026, 6, 14, 9, 40, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 10, IpAddress = "31.223.78.55",   SearchedProduct = "zeytinyağı",          SearchedAt = new DateTime(2026, 6, 14, 12, 15, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 11, IpAddress = "95.70.130.44",   SearchedProduct = "robot süpürge",       SearchedAt = new DateTime(2026, 6, 15, 10, 5, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 12, IpAddress = "85.102.45.12",   SearchedProduct = "kulaklık",            SearchedAt = new DateTime(2026, 6, 15, 17, 30, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 13, IpAddress = "212.174.60.33",  SearchedProduct = "nevresim",            SearchedAt = new DateTime(2026, 6, 16, 8, 50, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 14, IpAddress = "78.160.32.88",   SearchedProduct = "akıllı saat",         SearchedAt = new DateTime(2026, 6, 16, 14, 0, 0, DateTimeKind.Utc) },
            new SearchLog { Id = 15, IpAddress = "176.42.115.200", SearchedProduct = "deterjan",            SearchedAt = new DateTime(2026, 6, 17, 11, 20, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<UserCoordinate>().HasData(
            new UserCoordinate { Id = 1,  IpAddress = "85.102.45.12",   Latitude = 41.0082, Longitude = 28.9784, SavedAt = new DateTime(2026, 6, 10, 9, 15, 0, DateTimeKind.Utc) },
            new UserCoordinate { Id = 2,  IpAddress = "78.160.32.88",   Latitude = 39.9334, Longitude = 32.8597, SavedAt = new DateTime(2026, 6, 10, 10, 30, 0, DateTimeKind.Utc) },
            new UserCoordinate { Id = 3,  IpAddress = "176.42.115.200", Latitude = 38.4192, Longitude = 27.1287, SavedAt = new DateTime(2026, 6, 11, 14, 45, 0, DateTimeKind.Utc) },
            new UserCoordinate { Id = 4,  IpAddress = "31.223.78.55",   Latitude = 37.0000, Longitude = 35.3213, SavedAt = new DateTime(2026, 6, 12, 8, 0, 0, DateTimeKind.Utc) },
            new UserCoordinate { Id = 5,  IpAddress = "95.70.130.44",   Latitude = 36.8969, Longitude = 30.7133, SavedAt = new DateTime(2026, 6, 12, 11, 10, 0, DateTimeKind.Utc) },
            new UserCoordinate { Id = 6,  IpAddress = "212.174.60.33",  Latitude = 40.1885, Longitude = 29.0610, SavedAt = new DateTime(2026, 6, 14, 9, 40, 0, DateTimeKind.Utc) },
            new UserCoordinate { Id = 7,  IpAddress = "85.102.45.12",   Latitude = 41.0082, Longitude = 28.9784, SavedAt = new DateTime(2026, 6, 15, 17, 30, 0, DateTimeKind.Utc) },
            new UserCoordinate { Id = 8,  IpAddress = "78.160.32.88",   Latitude = 39.9334, Longitude = 32.8597, SavedAt = new DateTime(2026, 6, 16, 14, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<NotificationRequest>().HasData(
            new NotificationRequest { Id = 1,  IpAddress = "85.102.45.12",   Email = "ahmet.yilmaz@gmail.com",   SearchedProduct = "mangal",            RequestedAt = new DateTime(2026, 6, 10, 9, 16, 0, DateTimeKind.Utc) },
            new NotificationRequest { Id = 2,  IpAddress = "78.160.32.88",   Email = "elif.demir@hotmail.com",   SearchedProduct = "powerbank 30000",   RequestedAt = new DateTime(2026, 6, 10, 10, 32, 0, DateTimeKind.Utc) },
            new NotificationRequest { Id = 3,  IpAddress = "176.42.115.200", Email = "mehmet.kaya@outlook.com",  SearchedProduct = "çamaşır makinesi",  RequestedAt = new DateTime(2026, 6, 11, 14, 50, 0, DateTimeKind.Utc) },
            new NotificationRequest { Id = 4,  IpAddress = "31.223.78.55",   Email = "zeynep.celik@gmail.com",   SearchedProduct = "hava fritözü",      RequestedAt = new DateTime(2026, 6, 12, 8, 5, 0, DateTimeKind.Utc) },
            new NotificationRequest { Id = 5,  IpAddress = "95.70.130.44",   Email = "can.ozturk@yandex.com",    SearchedProduct = "çadır 4 kişilik",   RequestedAt = new DateTime(2026, 6, 12, 11, 15, 0, DateTimeKind.Utc) },
            new NotificationRequest { Id = 6,  IpAddress = "212.174.60.33",  Email = "ayse.sahin@gmail.com",     SearchedProduct = "güneş kremi",       RequestedAt = new DateTime(2026, 6, 14, 9, 42, 0, DateTimeKind.Utc) },
            new NotificationRequest { Id = 7,  IpAddress = "85.102.45.12",   Email = "ahmet.yilmaz@gmail.com",   SearchedProduct = "kulaklık bluetooth",RequestedAt = new DateTime(2026, 6, 15, 17, 35, 0, DateTimeKind.Utc) },
            new NotificationRequest { Id = 8,  IpAddress = "78.160.32.88",   Email = "elif.demir@hotmail.com",   SearchedProduct = "akıllı saat",       RequestedAt = new DateTime(2026, 6, 16, 14, 5, 0, DateTimeKind.Utc) },
            new NotificationRequest { Id = 9,  IpAddress = "176.42.115.200", Email = "mehmet.kaya@outlook.com",  SearchedProduct = "robot süpürge",     RequestedAt = new DateTime(2026, 6, 17, 11, 25, 0, DateTimeKind.Utc) },
            new NotificationRequest { Id = 10, IpAddress = "31.223.78.55",   Email = "zeynep.celik@gmail.com",   SearchedProduct = "bisiklet çocuk",    RequestedAt = new DateTime(2026, 6, 17, 15, 40, 0, DateTimeKind.Utc) }
        );
    }
}
