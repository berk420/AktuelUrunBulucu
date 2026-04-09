using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AktuelUrunBulucu.Migrations
{
    /// <inheritdoc />
    public partial class ResetProductIdSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SELECT SETVAL(pg_get_serial_sequence('public.products', 'id'), GREATEST((SELECT MAX(id) FROM public.products), 55));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE public.products ALTER COLUMN id RESTART WITH 1;");
        }
    }
}
