using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AktuelUrunBulucu.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "products",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    store_name = table.Column<string>(type: "text", nullable: false),
                    product_bring_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "search_logs",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ip_address = table.Column<string>(type: "text", nullable: false),
                    searched_product = table.Column<string>(type: "text", nullable: false),
                    searched_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_search_logs", x => x.id);
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
                    { 30, "Yaz & Havuz", "Güneş Şemsiyesi 2m UV Korumalı", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "CarrefourSA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "products",
                schema: "public");

            migrationBuilder.DropTable(
                name: "search_logs",
                schema: "public");
        }
    }
}
