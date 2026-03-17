using AktuelUrunBulucu.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AktuelUrunBulucu.DAL.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<SearchLog> SearchLogs => Set<SearchLog>();
    public DbSet<UserCoordinate> UserCoordinates => Set<UserCoordinate>();

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

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1,  Name = "Çamaşır Makinesi Samsung 8kg",       Category = "Beyaz Eşya",           StoreName = "Migros",      ProductBringDate = new DateTime(2026, 3,  1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 2,  Name = "Bulaşık Makinesi Arçelik 5 Program", Category = "Beyaz Eşya",           StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 3,  5, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 3,  Name = "Buzdolabı Vestel No-Frost",           Category = "Beyaz Eşya",           StoreName = "Migros",      ProductBringDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 4,  Name = "Mikrodalga Fırın 20L",                Category = "Küçük Ev Aleti",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 5,  Name = "Elektrikli Süpürge Rowenta",          Category = "Küçük Ev Aleti",       StoreName = "A101",        ProductBringDate = new DateTime(2026, 3,  8, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 6,  Name = "Hava Fritözü 5L",                     Category = "Küçük Ev Aleti",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 3, 12, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 7,  Name = "Bisiklet 26 Jant Dağ Bisikleti",      Category = "Spor & Outdoor",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4,  1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 8,  Name = "Bisiklet Çocuk 20 Jant",              Category = "Spor & Outdoor",       StoreName = "Migros",      ProductBringDate = new DateTime(2026, 4,  5, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 9,  Name = "Çadır 4 Kişilik Kamp Çadırı",         Category = "Kamp & Outdoor",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 10, Name = "Çadır 2 Kişilik Ultra Hafif",          Category = "Kamp & Outdoor",       StoreName = "Migros",      ProductBringDate = new DateTime(2026, 4, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 11, Name = "Barbekü Izgara Kömürlü Set",           Category = "Bahçe & Piknik",       StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 12, Name = "Barbekü Izgara Gazlı Taşınabilir",     Category = "Bahçe & Piknik",       StoreName = "A101",        ProductBringDate = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 13, Name = "Bahçe Hortumu 25m",                    Category = "Bahçe & Piknik",       StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4,  1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 14, Name = "Çim Biçme Makinesi Elektrikli",        Category = "Bahçe & Piknik",       StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4, 20, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 15, Name = "Bahçe Masa Sandalye Seti 4+1",         Category = "Mobilya & Dekorasyon", StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 4,  5, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 16, Name = "Katlanır Kamp Sandalyesi",             Category = "Kamp & Outdoor",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4,  1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 17, Name = "Raf Sistemi Metal 5 Katlı",            Category = "Mobilya & Dekorasyon", StoreName = "A101",        ProductBringDate = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 18, Name = "Uyku Tulumu -5 Derece",                Category = "Kamp & Outdoor",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4,  1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 19, Name = "Yağmurluk Unisex L Beden",             Category = "Giyim",                StoreName = "BİM",         ProductBringDate = new DateTime(2026, 3, 25, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 20, Name = "Spor Ayakkabı Erkek 42",               Category = "Giyim",                StoreName = "Migros",      ProductBringDate = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 21, Name = "Akıllı Saat Fitness Tracker",           Category = "Elektronik",           StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 22, Name = "Bluetooth Hoparlör Su Geçirmez",        Category = "Elektronik",           StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 23, Name = "Powerbank 20000mAh",                    Category = "Elektronik",           StoreName = "A101",        ProductBringDate = new DateTime(2026, 3,  5, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 24, Name = "Çocuk Scooter 3 Tekerlekli",            Category = "Çocuk & Oyuncak",      StoreName = "BİM",         ProductBringDate = new DateTime(2026, 4,  5, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 25, Name = "Kaydırak Çocuk Bahçe Seti",             Category = "Çocuk & Oyuncak",      StoreName = "A101",        ProductBringDate = new DateTime(2026, 4, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 26, Name = "Matkap Seti Akülü 18V",                 Category = "Araç Gereç",           StoreName = "Şok",         ProductBringDate = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 27, Name = "El Arabası Plastik Bahçe",              Category = "Bahçe & Piknik",       StoreName = "Şok",         ProductBringDate = new DateTime(2026, 4, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 28, Name = "Merdiven 5 Basamak Alüminyum",          Category = "Araç Gereç",           StoreName = "Migros",      ProductBringDate = new DateTime(2026, 3, 25, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 29, Name = "Şişme Havuz 300x200cm Aile",            Category = "Yaz & Havuz",          StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 5,  1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 30, Name = "Güneş Şemsiyesi 2m UV Korumalı",        Category = "Yaz & Havuz",          StoreName = "CarrefourSA", ProductBringDate = new DateTime(2026, 5,  1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
