using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Estatistica.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogConcorrenteProdutos_AspNetUsers_UsuarioId",
                table: "LogConcorrenteProdutos");

            migrationBuilder.DropIndex(
                name: "IX_LogConcorrenteProdutos_UsuarioId",
                table: "LogConcorrenteProdutos");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab4d57fd-d157-473b-b0e7-45aa9cb3b672");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b24d88df-458b-4796-b226-b5ed67610dc6");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "LogConcorrenteProdutos");

            migrationBuilder.DropColumn(
                name: "dtInclusao",
                table: "LogConcorrenteProdutos");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cf1264dc-65d7-4612-8ff3-c1fd3a719ebd", null, "Admin", "Admin" },
                    { "ed2b92a5-01a9-4fb3-b0ec-55108a69a8dd", null, "RCA", "RCA" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf1264dc-65d7-4612-8ff3-c1fd3a719ebd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed2b92a5-01a9-4fb3-b0ec-55108a69a8dd");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "LogConcorrenteProdutos",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dtInclusao",
                table: "LogConcorrenteProdutos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ab4d57fd-d157-473b-b0e7-45aa9cb3b672", null, "Admin", "Admin" },
                    { "b24d88df-458b-4796-b226-b5ed67610dc6", null, "RCA", "RCA" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogConcorrenteProdutos_UsuarioId",
                table: "LogConcorrenteProdutos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogConcorrenteProdutos_AspNetUsers_UsuarioId",
                table: "LogConcorrenteProdutos",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
