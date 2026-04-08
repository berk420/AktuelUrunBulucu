using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AktuelUrunBulucu.Migrations
{
    /// <inheritdoc />
    public partial class AddPresentationSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "products",
                columns: new[] { "id", "category", "name", "product_bring_date", "store_name" },
                values: new object[,]
                {
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
                    { 55, "Elektronik", "Powerbank 25000mAh 4 Portlu Hızlı Şarj", new DateTime(2026, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Şok" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 55);
        }
    }
}
