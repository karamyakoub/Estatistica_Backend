using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Estatistica.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class changelogconcorrenteproduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d4f6ea4-7d82-4f27-8cd0-73e7a4ee0d0b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c2648cc-c643-4b43-abce-1958d666b1bb");

            migrationBuilder.AddColumn<string>(
                name: "CodigoProdutoAtualCodigoProduto",
                table: "LogConcorrenteProdutos",
                type: "varchar(15)",
                nullable: true);

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
                name: "IX_LogConcorrenteProdutos_CodigoProdutoAtualCodigoProduto",
                table: "LogConcorrenteProdutos",
                column: "CodigoProdutoAtualCodigoProduto");

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

            migrationBuilder.AddForeignKey(
                name: "FK_LogConcorrenteProdutos_Produtos_CodigoProdutoAtualCodigoProd~",
                table: "LogConcorrenteProdutos",
                column: "CodigoProdutoAtualCodigoProduto",
                principalTable: "Produtos",
                principalColumn: "codProd");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogConcorrenteProdutos_AspNetUsers_UsuarioId",
                table: "LogConcorrenteProdutos");

            migrationBuilder.DropForeignKey(
                name: "FK_LogConcorrenteProdutos_Produtos_CodigoProdutoAtualCodigoProd~",
                table: "LogConcorrenteProdutos");

            migrationBuilder.DropIndex(
                name: "IX_LogConcorrenteProdutos_CodigoProdutoAtualCodigoProduto",
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
                name: "CodigoProdutoAtualCodigoProduto",
                table: "LogConcorrenteProdutos");

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
                    { "7d4f6ea4-7d82-4f27-8cd0-73e7a4ee0d0b", null, "Admin", "Admin" },
                    { "9c2648cc-c643-4b43-abce-1958d666b1bb", null, "RCA", "RCA" }
                });
        }
    }
}
