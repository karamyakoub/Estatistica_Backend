using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Estatistica.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class includeprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf1264dc-65d7-4612-8ff3-c1fd3a719ebd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed2b92a5-01a9-4fb3-b0ec-55108a69a8dd");

            migrationBuilder.AddColumn<decimal>(
                name: "custo",
                table: "Produtos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "pVenda",
                table: "Produtos",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "codMuni",
                table: "Nfis",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fretes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    setor = table.Column<string>(type: "longtext", nullable: false),
                    CodMuni = table.Column<string>(type: "longtext", nullable: false),
                    percFrete = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fretes", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "43b63863-3fef-4c40-81c9-4a0a621569a1", null, "RCA", "RCA" },
                    { "a6e95cf3-1a0d-40c3-b646-b3d673b38023", null, "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fretes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43b63863-3fef-4c40-81c9-4a0a621569a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6e95cf3-1a0d-40c3-b646-b3d673b38023");

            migrationBuilder.DropColumn(
                name: "custo",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "pVenda",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "codMuni",
                table: "Nfis");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cf1264dc-65d7-4612-8ff3-c1fd3a719ebd", null, "Admin", "Admin" },
                    { "ed2b92a5-01a9-4fb3-b0ec-55108a69a8dd", null, "RCA", "RCA" }
                });
        }
    }
}
