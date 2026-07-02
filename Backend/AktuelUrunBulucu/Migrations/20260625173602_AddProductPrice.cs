using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AktuelUrunBulucu.Migrations
{
    /// <inheritdoc />
    public partial class AddProductPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "product_price",
                schema: "public",
                table: "products",
                type: "numeric",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "product_price",
                value: 12999m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "product_price",
                value: 9499m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "product_price",
                value: 15999m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 4,
                column: "product_price",
                value: 1299m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 5,
                column: "product_price",
                value: 2499m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 6,
                column: "product_price",
                value: 1899m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 7,
                column: "product_price",
                value: 4999m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 8,
                column: "product_price",
                value: 2799m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 9,
                column: "product_price",
                value: 3499m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 10,
                column: "product_price",
                value: 2199m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 11,
                column: "product_price",
                value: 899m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 12,
                column: "product_price",
                value: 1499m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 13,
                column: "product_price",
                value: 349m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 14,
                column: "product_price",
                value: 2999m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 15,
                column: "product_price",
                value: 3799m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 16,
                column: "product_price",
                value: 299m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 17,
                column: "product_price",
                value: 1199m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 18,
                column: "product_price",
                value: 799m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 19,
                column: "product_price",
                value: 449m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 20,
                column: "product_price",
                value: 999m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 21,
                column: "product_price",
                value: 2299m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 22,
                column: "product_price",
                value: 899m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 23,
                column: "product_price",
                value: 499m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 24,
                column: "product_price",
                value: 699m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 25,
                column: "product_price",
                value: 1599m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 26,
                column: "product_price",
                value: 1799m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 27,
                column: "product_price",
                value: 599m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 28,
                column: "product_price",
                value: 1099m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 29,
                column: "product_price",
                value: 2499m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 30,
                column: "product_price",
                value: 799m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 31,
                column: "product_price",
                value: 649m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 32,
                column: "product_price",
                value: 899m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 33,
                column: "product_price",
                value: 499m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 34,
                column: "product_price",
                value: 1299m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 35,
                column: "product_price",
                value: 1099m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 36,
                column: "product_price",
                value: 149m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 37,
                column: "product_price",
                value: 249m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 38,
                column: "product_price",
                value: 79m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 39,
                column: "product_price",
                value: 199m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 40,
                column: "product_price",
                value: 169m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 41,
                column: "product_price",
                value: 99m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 42,
                column: "product_price",
                value: 349m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 43,
                column: "product_price",
                value: 129m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 44,
                column: "product_price",
                value: 59m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 45,
                column: "product_price",
                value: 399m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 46,
                column: "product_price",
                value: 199m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 47,
                column: "product_price",
                value: 349m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 48,
                column: "product_price",
                value: 499m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 49,
                column: "product_price",
                value: 899m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 50,
                column: "product_price",
                value: 699m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 51,
                column: "product_price",
                value: 749m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 52,
                column: "product_price",
                value: 599m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 53,
                column: "product_price",
                value: 129m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 54,
                column: "product_price",
                value: 299m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 55,
                column: "product_price",
                value: 799m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 56,
                column: "product_price",
                value: 849m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 57,
                column: "product_price",
                value: 699m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 58,
                column: "product_price",
                value: 549m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 59,
                column: "product_price",
                value: 399m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 60,
                column: "product_price",
                value: 249m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 61,
                column: "product_price",
                value: 179m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 62,
                column: "product_price",
                value: 1299m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 63,
                column: "product_price",
                value: 999m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 64,
                column: "product_price",
                value: 599m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 65,
                column: "product_price",
                value: 799m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 66,
                column: "product_price",
                value: 349m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 67,
                column: "product_price",
                value: 499m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 68,
                column: "product_price",
                value: 449m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 69,
                column: "product_price",
                value: 299m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 70,
                column: "product_price",
                value: 8999m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 71,
                column: "product_price",
                value: 129m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 72,
                column: "product_price",
                value: 599m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 73,
                column: "product_price",
                value: 899m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 74,
                column: "product_price",
                value: 1499m);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "products",
                keyColumn: "id",
                keyValue: 75,
                column: "product_price",
                value: 3499m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "product_price",
                schema: "public",
                table: "products");
        }
    }
}
