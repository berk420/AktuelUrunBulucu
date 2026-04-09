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
            migrationBuilder.Sql(@"
                INSERT INTO public.products (id, category, name, product_bring_date, store_name) VALUES
                (31, 'Bahçe & Piknik', 'Mangal Kömürlü Set Takım Çantalı',     '2026-04-08T00:00:00Z', 'BİM'),
                (32, 'Bahçe & Piknik', 'Mangal Büyük Boy Aile Mangalı',          '2026-04-08T00:00:00Z', 'A101'),
                (33, 'Bahçe & Piknik', 'Mangal Portatif Katlanır Kamp Mangalı',  '2026-04-08T00:00:00Z', 'Migros'),
                (34, 'Bahçe & Piknik', 'Mangal Gazlı Taşınabilir 2 Gözlü',       '2026-04-08T00:00:00Z', 'CarrefourSA'),
                (35, 'Bahçe & Piknik', 'Mangal Elektrikli İç Mekan 2000W',       '2026-04-08T00:00:00Z', 'Şok'),
                (36, 'Bahçe & Piknik', 'Mangal Kömürü 5kg Doğal Meyve Odunu',    '2026-04-08T00:00:00Z', 'BİM'),
                (37, 'Bahçe & Piknik', 'Mangal Kömürü 10kg Premium',             '2026-04-08T00:00:00Z', 'A101'),
                (38, 'Bahçe & Piknik', 'Mangal Ateş Tutuşturucu Jel 500ml',      '2026-04-08T00:00:00Z', 'Migros'),
                (39, 'Bahçe & Piknik', 'Mangal Izgara Teli 40x60cm Paslanmaz',   '2026-04-08T00:00:00Z', 'CarrefourSA'),
                (40, 'Bahçe & Piknik', 'Mangal Maşa Spatula Set 5 Parça',        '2026-04-08T00:00:00Z', 'Şok'),
                (41, 'Bahçe & Piknik', 'Mangal Eldiveni Isıya Dayanıklı Çift',   '2026-04-08T00:00:00Z', 'BİM'),
                (42, 'Bahçe & Piknik', 'Mangal Izgarası Döküm Demir 50cm',       '2026-04-08T00:00:00Z', 'A101'),
                (43, 'Bahçe & Piknik', 'Mangal Barbekü Sis Şişi 12li Set',       '2026-04-08T00:00:00Z', 'Migros'),
                (44, 'Bahçe & Piknik', 'Mangal Alüminyum Folyo Tepsi 5li',       '2026-04-08T00:00:00Z', 'CarrefourSA'),
                (45, 'Bahçe & Piknik', 'Mangal Çantalı Piknik Seti 20 Parça',    '2026-04-08T00:00:00Z', 'Şok'),
                (46, 'Elektronik',     'Powerbank 5000mAh Slim Taşınabilir',      '2026-04-08T00:00:00Z', 'BİM'),
                (47, 'Elektronik',     'Powerbank 10000mAh Hızlı Şarj 22.5W',    '2026-04-08T00:00:00Z', 'A101'),
                (48, 'Elektronik',     'Powerbank 20000mAh Çift USB Çıkış',       '2026-04-08T00:00:00Z', 'Migros'),
                (49, 'Elektronik',     'Powerbank 20000mAh PD 65W Laptop',        '2026-04-08T00:00:00Z', 'CarrefourSA'),
                (50, 'Elektronik',     'Powerbank 30000mAh Süper Kapasite',       '2026-04-08T00:00:00Z', 'Şok'),
                (51, 'Elektronik',     'Powerbank Kablosuz Şarjlı 15W MagSafe',  '2026-04-08T00:00:00Z', 'BİM'),
                (52, 'Elektronik',     'Powerbank Solar Güneş Enerjili 10000mAh', '2026-04-08T00:00:00Z', 'A101'),
                (53, 'Elektronik',     'Powerbank Mini Anahtarlık 1500mAh',       '2026-04-08T00:00:00Z', 'Migros'),
                (54, 'Elektronik',     'Powerbank 10000mAh Led Göstergeli',       '2026-04-08T00:00:00Z', 'CarrefourSA'),
                (55, 'Elektronik',     'Powerbank 25000mAh 4 Portlu Hızlı Şarj', '2026-04-08T00:00:00Z', 'Şok')
                ON CONFLICT (id) DO NOTHING;
            ");
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
