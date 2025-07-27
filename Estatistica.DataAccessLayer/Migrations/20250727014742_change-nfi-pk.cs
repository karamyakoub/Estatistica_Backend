using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Estatistica.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class changenfipk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47d70087-2cde-4775-915f-92e052b9fd11");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f19a226d-d0d7-4cb8-b3ce-f9818c3efd77");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Nfis",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "289c2d79-f8cc-4be8-a124-77aa2a9f333b", null, "Admin", "Admin" },
                    { "4d25e68f-f133-4885-9bd2-96f9b460fa0d", null, "RCA", "RCA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "289c2d79-f8cc-4be8-a124-77aa2a9f333b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d25e68f-f133-4885-9bd2-96f9b460fa0d");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Nfis",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "47d70087-2cde-4775-915f-92e052b9fd11", null, "Admin", "Admin" },
                    { "f19a226d-d0d7-4cb8-b3ce-f9818c3efd77", null, "RCA", "RCA" }
                });
        }
    }
}
