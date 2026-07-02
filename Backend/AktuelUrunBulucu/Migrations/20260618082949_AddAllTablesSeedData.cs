using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AktuelUrunBulucu.Migrations
{
    /// <inheritdoc />
    public partial class AddAllTablesSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE public.notification_requests, public.search_logs, public.user_coordinates, public.products RESTART IDENTITY CASCADE;");

            migrationBuilder.InsertData(
                schema: "public",
                table: "notification_requests",
                columns: new[] { "id", "email", "ip_address", "requested_at", "searched_product" },
                values: new object[,]
                {
                    { 1, "ahmet.yilmaz@gmail.com", "85.102.45.12", new DateTime(2026, 6, 10, 9, 16, 0, 0, DateTimeKind.Utc), "mangal" },
                    { 2, "elif.demir@hotmail.com", "78.160.32.88", new DateTime(2026, 6, 10, 10, 32, 0, 0, DateTimeKind.Utc), "powerbank 30000" },
                    { 3, "mehmet.kaya@outlook.com", "176.42.115.200", new DateTime(2026, 6, 11, 14, 50, 0, 0, DateTimeKind.Utc), "çamaşır makinesi" },
                    { 4, "zeynep.celik@gmail.com", "31.223.78.55", new DateTime(2026, 6, 12, 8, 5, 0, 0, DateTimeKind.Utc), "hava fritözü" },
                    { 5, "can.ozturk@yandex.com", "95.70.130.44", new DateTime(2026, 6, 12, 11, 15, 0, 0, DateTimeKind.Utc), "çadır 4 kişilik" },
                    { 6, "ayse.sahin@gmail.com", "212.174.60.33", new DateTime(2026, 6, 14, 9, 42, 0, 0, DateTimeKind.Utc), "güneş kremi" },
                    { 7, "ahmet.yilmaz@gmail.com", "85.102.45.12", new DateTime(2026, 6, 15, 17, 35, 0, 0, DateTimeKind.Utc), "kulaklık bluetooth" },
                    { 8, "elif.demir@hotmail.com", "78.160.32.88", new DateTime(2026, 6, 16, 14, 5, 0, 0, DateTimeKind.Utc), "akıllı saat" },
                    { 9, "mehmet.kaya@outlook.com", "176.42.115.200", new DateTime(2026, 6, 17, 11, 25, 0, 0, DateTimeKind.Utc), "robot süpürge" },
                    { 10, "zeynep.celik@gmail.com", "31.223.78.55", new DateTime(2026, 6, 17, 15, 40, 0, 0, DateTimeKind.Utc), "bisiklet çocuk" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "products",
                columns: new[] { "id", "category", "name", "product_bring_date", "store_name" },
                values: new object[,]
                {
                    { 1, "Beyaz Eşya", "Çamaşır Makinesi Samsung 8kg", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 2, "Beyaz Eşya", "Bulaşık Makinesi Arçelik 5 Program", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 3, "Beyaz Eşya", "Buzdolabı Vestel No-Frost", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 4, "Küçük Ev Aleti", "Mikrodalga Fırın 20L", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 5, "Küçük Ev Aleti", "Elektrikli Süpürge Rowenta", new DateTime(2026, 3, 8, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 6, "Küçük Ev Aleti", "Hava Fritözü 5L", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 7, "Spor & Outdoor", "Bisiklet 26 Jant Dağ Bisikleti", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 8, "Spor & Outdoor", "Bisiklet Çocuk 20 Jant", new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 9, "Kamp & Outdoor", "Çadır 4 Kişilik Kamp Çadırı", new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 10, "Kamp & Outdoor", "Çadır 2 Kişilik Ultra Hafif", new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 11, "Bahçe & Piknik", "Barbekü Izgara Kömürlü Set", new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 12, "Bahçe & Piknik", "Barbekü Izgara Gazlı Taşınabilir", new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 13, "Bahçe & Piknik", "Bahçe Hortumu 25m", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 14, "Bahçe & Piknik", "Çim Biçme Makinesi Elektrikli", new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 15, "Mobilya & Dekorasyon", "Bahçe Masa Sandalye Seti 4+1", new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 16, "Kamp & Outdoor", "Katlanır Kamp Sandalyesi", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 17, "Mobilya & Dekorasyon", "Raf Sistemi Metal 5 Katlı", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 18, "Kamp & Outdoor", "Uyku Tulumu -5 Derece", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 19, "Giyim", "Yağmurluk Unisex L Beden", new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 20, "Giyim", "Spor Ayakkabı Erkek 42", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 21, "Elektronik", "Akıllı Saat Fitness Tracker", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 22, "Elektronik", "Bluetooth Hoparlör Su Geçirmez", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 23, "Elektronik", "Powerbank 20000mAh", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 24, "Çocuk & Oyuncak", "Çocuk Scooter 3 Tekerlekli", new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 25, "Çocuk & Oyuncak", "Kaydırak Çocuk Bahçe Seti", new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 26, "Araç Gereç", "Matkap Seti Akülü 18V", new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 27, "Bahçe & Piknik", "El Arabası Plastik Bahçe", new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 28, "Araç Gereç", "Merdiven 5 Basamak Alüminyum", new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 29, "Yaz & Havuz", "Şişme Havuz 300x200cm Aile", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 30, "Yaz & Havuz", "Güneş Şemsiyesi 2m UV Korumalı", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 31, "Bahçe & Piknik", "Mangal Kömürlü Set Takım Çantalı", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 32, "Bahçe & Piknik", "Mangal Büyük Boy Aile Mangalı", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 33, "Bahçe & Piknik", "Mangal Portatif Katlanır Kamp Mangalı", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 34, "Bahçe & Piknik", "Mangal Gazlı Taşınabilir 2 Gözlü", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 35, "Bahçe & Piknik", "Mangal Elektrikli İç Mekan 2000W", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 36, "Bahçe & Piknik", "Mangal Kömürü 5kg Doğal Meyve Odunu", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 37, "Bahçe & Piknik", "Mangal Kömürü 10kg Premium", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 38, "Bahçe & Piknik", "Mangal Ateş Tutuşturucu Jel 500ml", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 39, "Bahçe & Piknik", "Mangal Izgara Teli 40x60cm Paslanmaz", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 40, "Bahçe & Piknik", "Mangal Maşa Spatula Set 5 Parça", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 41, "Bahçe & Piknik", "Mangal Eldiveni Isıya Dayanıklı Çift", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 42, "Bahçe & Piknik", "Mangal Izgarası Döküm Demir 50cm", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 43, "Bahçe & Piknik", "Mangal Barbekü Sis Şişi 12li Set", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 44, "Bahçe & Piknik", "Mangal Alüminyum Folyo Tepsi 5li", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 45, "Bahçe & Piknik", "Mangal Çantalı Piknik Seti 20 Parça", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 46, "Elektronik", "Powerbank 5000mAh Slim Taşınabilir", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 47, "Elektronik", "Powerbank 10000mAh Hızlı Şarj 22.5W", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 48, "Elektronik", "Powerbank 20000mAh Çift USB Çıkış", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 49, "Elektronik", "Powerbank 20000mAh PD 65W Laptop", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 50, "Elektronik", "Powerbank 30000mAh Süper Kapasite", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 51, "Elektronik", "Powerbank Kablosuz Şarjlı 15W MagSafe", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 52, "Elektronik", "Powerbank Solar Güneş Enerjili 10000mAh", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 53, "Elektronik", "Powerbank Mini Anahtarlık 1500mAh", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 54, "Elektronik", "Powerbank 10000mAh Led Göstergeli", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 55, "Elektronik", "Powerbank 25000mAh 4 Portlu Hızlı Şarj", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 56, "Gıda", "Zeytinyağı Riviera 5L Teneke", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 57, "Gıda", "Fındık İç 1kg Giresun", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 58, "Gıda", "Bal Süzme Çam Balı 850g", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 59, "Gıda", "Çay Rize Çayı 3kg Karton Kutu", new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 60, "Gıda", "Türk Kahvesi 500g Öğütülmüş", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 61, "Kozmetik", "Güneş Kremi SPF50 200ml", new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 62, "Kozmetik", "Saç Kurutma Makinesi 2200W İyonik", new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 63, "Kozmetik", "Epilatör Su Geçirmez Şarjlı", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 64, "Kozmetik", "Parfüm Erkek EDT 100ml", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 65, "Ev Tekstili", "Nevresim Takımı Çift Kişilik Pamuk", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 66, "Ev Tekstili", "Havlu Seti 6lı Banyo Havlusu", new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 67, "Ev Tekstili", "Yastık Visco Ortopedik Boyun", new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 68, "Temizlik", "Çamaşır Deterjanı 10kg Toz", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 69, "Temizlik", "Bulaşık Deterjanı Tablet 72li", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 70, "Küçük Ev Aleti", "Robot Süpürge Akıllı Lazerli", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" },
                    { 71, "Otomotiv", "Araç İçi Telefon Tutucu Manyetik", new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), "A101" },
                    { 72, "Otomotiv", "Araç Kompresörü 12V Dijital", new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), "BİM" },
                    { 73, "Otomotiv", "Araç Koltuk Kılıfı Deri Universal", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" },
                    { 74, "Elektronik", "Tablet Kalem Stylus iPad Uyumlu", new DateTime(2026, 6, 22, 0, 0, 0, 0, DateTimeKind.Utc), "Migros" },
                    { 75, "Elektronik", "Kulaklık Bluetooth ANC Gürültü Önleyici", new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "search_logs",
                columns: new[] { "id", "ip_address", "searched_at", "searched_product" },
                values: new object[,]
                {
                    { 1, "85.102.45.12", new DateTime(2026, 6, 10, 9, 15, 0, 0, DateTimeKind.Utc), "mangal" },
                    { 2, "78.160.32.88", new DateTime(2026, 6, 10, 10, 30, 0, 0, DateTimeKind.Utc), "powerbank" },
                    { 3, "176.42.115.200", new DateTime(2026, 6, 11, 14, 45, 0, 0, DateTimeKind.Utc), "çamaşır makinesi" },
                    { 4, "85.102.45.12", new DateTime(2026, 6, 11, 16, 20, 0, 0, DateTimeKind.Utc), "bisiklet" },
                    { 5, "31.223.78.55", new DateTime(2026, 6, 12, 8, 0, 0, 0, DateTimeKind.Utc), "hava fritözü" },
                    { 6, "95.70.130.44", new DateTime(2026, 6, 12, 11, 10, 0, 0, DateTimeKind.Utc), "çadır" },
                    { 7, "78.160.32.88", new DateTime(2026, 6, 13, 13, 25, 0, 0, DateTimeKind.Utc), "bluetooth hoparlör" },
                    { 8, "176.42.115.200", new DateTime(2026, 6, 13, 15, 55, 0, 0, DateTimeKind.Utc), "buzdolabı" },
                    { 9, "212.174.60.33", new DateTime(2026, 6, 14, 9, 40, 0, 0, DateTimeKind.Utc), "güneş kremi" },
                    { 10, "31.223.78.55", new DateTime(2026, 6, 14, 12, 15, 0, 0, DateTimeKind.Utc), "zeytinyağı" },
                    { 11, "95.70.130.44", new DateTime(2026, 6, 15, 10, 5, 0, 0, DateTimeKind.Utc), "robot süpürge" },
                    { 12, "85.102.45.12", new DateTime(2026, 6, 15, 17, 30, 0, 0, DateTimeKind.Utc), "kulaklık" },
                    { 13, "212.174.60.33", new DateTime(2026, 6, 16, 8, 50, 0, 0, DateTimeKind.Utc), "nevresim" },
                    { 14, "78.160.32.88", new DateTime(2026, 6, 16, 14, 0, 0, 0, DateTimeKind.Utc), "akıllı saat" },
                    { 15, "176.42.115.200", new DateTime(2026, 6, 17, 11, 20, 0, 0, DateTimeKind.Utc), "deterjan" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "user_coordinates",
                columns: new[] { "id", "ip_address", "latitude", "longitude", "saved_at" },
                values: new object[,]
                {
                    { 1, "85.102.45.12", 41.008200000000002, 28.978400000000001, new DateTime(2026, 6, 10, 9, 15, 0, 0, DateTimeKind.Utc) },
                    { 2, "78.160.32.88", 39.933399999999999, 32.859699999999997, new DateTime(2026, 6, 10, 10, 30, 0, 0, DateTimeKind.Utc) },
                    { 3, "176.42.115.200", 38.419199999999996, 27.128699999999998, new DateTime(2026, 6, 11, 14, 45, 0, 0, DateTimeKind.Utc) },
                    { 4, "31.223.78.55", 37.0, 35.321300000000001, new DateTime(2026, 6, 12, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "95.70.130.44", 36.896900000000002, 30.7133, new DateTime(2026, 6, 12, 11, 10, 0, 0, DateTimeKind.Utc) },
                    { 6, "212.174.60.33", 40.188499999999998, 29.061, new DateTime(2026, 6, 14, 9, 40, 0, 0, DateTimeKind.Utc) },
                    { 7, "85.102.45.12", 41.008200000000002, 28.978400000000001, new DateTime(2026, 6, 15, 17, 30, 0, 0, DateTimeKind.Utc) },
                    { 8, "78.160.32.88", 39.933399999999999, 32.859699999999997, new DateTime(2026, 6, 16, 14, 0, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "notification_requests",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "notification_requests",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "notification_requests",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "notification_requests",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "notification_requests",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "notification_requests",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "notification_requests",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "notification_requests",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "notification_requests",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "notification_requests",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "search_logs",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "user_coordinates",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "user_coordinates",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "user_coordinates",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "user_coordinates",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "user_coordinates",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "user_coordinates",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "user_coordinates",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "user_coordinates",
                keyColumn: "id",
                keyValue: 8);
        }
    }
}
