using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Estatistica.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class qtdecorrecao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "43b63863-3fef-4c40-81c9-4a0a621569a1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6e95cf3-1a0d-40c3-b646-b3d673b38023");

            migrationBuilder.AddColumn<int>(
                name: "qtdeCorrecao",
                table: "Nfis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "597313e9-c06c-4872-b014-dea91a6297b3", null, "Admin", "Admin" },
                    { "6c803a7a-ebbc-4f08-afa8-f6d7de07a5b9", null, "RCA", "RCA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "597313e9-c06c-4872-b014-dea91a6297b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6c803a7a-ebbc-4f08-afa8-f6d7de07a5b9");

            migrationBuilder.DropColumn(
                name: "qtdeCorrecao",
                table: "Nfis");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "43b63863-3fef-4c40-81c9-4a0a621569a1", null, "RCA", "RCA" },
                    { "a6e95cf3-1a0d-40c3-b646-b3d673b38023", null, "Admin", "Admin" }
                });
        }
    }
}
