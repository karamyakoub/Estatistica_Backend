using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Estatistica.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Concorrentes",
                columns: table => new
                {
                    conId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    dtCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usuCadastro = table.Column<string>(type: "longtext", nullable: true),
                    dtAlter = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usuAlter = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concorrentes", x => x.conId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Planilhas",
                columns: table => new
                {
                    planilhaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nomePlanilha = table.Column<string>(type: "longtext", nullable: true),
                    caminho = table.Column<string>(type: "longtext", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    dtCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usuCadastro = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planilhas", x => x.planilhaId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    codProd = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    codFab = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    codBarra = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    descricao = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true),
                    fabricante = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true),
                    codMarca = table.Column<int>(type: "int", maxLength: 10, nullable: true),
                    descMarca = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true),
                    tipo = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true),
                    subtipo = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true),
                    linha = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true),
                    familia = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true),
                    unidade = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true),
                    dtCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.codProd);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConcorrenteFilials",
                columns: table => new
                {
                    cnpj = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    ConcorrenteId = table.Column<int>(type: "int", nullable: false),
                    dtCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usuCadastro = table.Column<string>(type: "longtext", nullable: true),
                    dtAlter = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usuAlter = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcorrenteFilials", x => x.cnpj);
                    table.ForeignKey(
                        name: "FK_ConcorrenteFilials_Concorrentes_ConcorrenteId",
                        column: x => x.ConcorrenteId,
                        principalTable: "Concorrentes",
                        principalColumn: "conId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConcorrenteFilialPendentes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cnpj = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    nome = table.Column<string>(type: "longtext", nullable: false),
                    PlanilhaId = table.Column<int>(type: "int", nullable: true),
                    ConcorrenteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcorrenteFilialPendentes", x => x.id);
                    table.ForeignKey(
                        name: "FK_ConcorrenteFilialPendentes_Concorrentes_ConcorrenteId",
                        column: x => x.ConcorrenteId,
                        principalTable: "Concorrentes",
                        principalColumn: "conId");
                    table.ForeignKey(
                        name: "FK_ConcorrenteFilialPendentes_Planilhas_PlanilhaId",
                        column: x => x.PlanilhaId,
                        principalTable: "Planilhas",
                        principalColumn: "planilhaId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConcorrenteFilialTemps",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cnpj = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    nome = table.Column<string>(type: "longtext", nullable: false),
                    PlanilhaId = table.Column<int>(type: "int", nullable: true),
                    incluido = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcorrenteFilialTemps", x => x.id);
                    table.ForeignKey(
                        name: "FK_ConcorrenteFilialTemps_Planilhas_PlanilhaId",
                        column: x => x.PlanilhaId,
                        principalTable: "Planilhas",
                        principalColumn: "planilhaId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlanilhaStatuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PlanhilaId = table.Column<int>(type: "int", nullable: false),
                    situacao = table.Column<int>(type: "int", nullable: false),
                    obs = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    dtInclusao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanilhaStatuses", x => x.id);
                    table.ForeignKey(
                        name: "FK_PlanilhaStatuses_Planilhas_PlanhilaId",
                        column: x => x.PlanhilaId,
                        principalTable: "Planilhas",
                        principalColumn: "planilhaId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConcorrenteProdutos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    ConcorrenteId = table.Column<int>(type: "int", nullable: false),
                    codProdCon = table.Column<string>(type: "longtext", nullable: false),
                    descProdCon = table.Column<string>(type: "longtext", nullable: false),
                    unProdCon = table.Column<string>(type: "longtext", nullable: true),
                    codBarraCon = table.Column<string>(type: "longtext", nullable: true),
                    ProdutoCodigoProduto = table.Column<string>(type: "varchar(15)", nullable: true),
                    PlanilhaId = table.Column<int>(type: "int", nullable: true),
                    tipoVinculo = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true),
                    dtCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usuCadastro = table.Column<string>(type: "longtext", nullable: true),
                    dtAlter = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usuAlter = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcorrenteProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConcorrenteProdutos_Concorrentes_ConcorrenteId",
                        column: x => x.ConcorrenteId,
                        principalTable: "Concorrentes",
                        principalColumn: "conId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConcorrenteProdutos_Planilhas_PlanilhaId",
                        column: x => x.PlanilhaId,
                        principalTable: "Planilhas",
                        principalColumn: "planilhaId");
                    table.ForeignKey(
                        name: "FK_ConcorrenteProdutos_Produtos_ProdutoCodigoProduto",
                        column: x => x.ProdutoCodigoProduto,
                        principalTable: "Produtos",
                        principalColumn: "codProd");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Nfcs",
                columns: table => new
                {
                    chaveNfe = table.Column<string>(type: "varchar(255)", nullable: false),
                    ConcorrenteCnpjCnpj = table.Column<string>(type: "varchar(14)", nullable: true),
                    dtEmissao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    cnpjCliente = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true),
                    nomeCliente = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PlanilhaId = table.Column<int>(type: "int", nullable: true),
                    dtCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usuCadastro = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nfcs", x => x.chaveNfe);
                    table.ForeignKey(
                        name: "FK_Nfcs_ConcorrenteFilials_ConcorrenteCnpjCnpj",
                        column: x => x.ConcorrenteCnpjCnpj,
                        principalTable: "ConcorrenteFilials",
                        principalColumn: "cnpj");
                    table.ForeignKey(
                        name: "FK_Nfcs_Planilhas_PlanilhaId",
                        column: x => x.PlanilhaId,
                        principalTable: "Planilhas",
                        principalColumn: "planilhaId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LogConcorrenteProdutos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ConcorrenteId = table.Column<int>(type: "int", nullable: false),
                    CodigoProdutoConcorrenteId = table.Column<string>(type: "varchar(255)", nullable: false),
                    CodigoProdutoAntCodigoProduto = table.Column<string>(type: "varchar(15)", nullable: true),
                    descProdConAnt = table.Column<string>(type: "longtext", nullable: false),
                    descProdConAtual = table.Column<string>(type: "longtext", nullable: false),
                    unProdConAnt = table.Column<string>(type: "longtext", nullable: true),
                    unProdConAtual = table.Column<string>(type: "longtext", nullable: true),
                    codBarraConAnt = table.Column<string>(type: "longtext", nullable: true),
                    codBarraConAtual = table.Column<string>(type: "longtext", nullable: true),
                    obs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    dtCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usuCadastro = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogConcorrenteProdutos", x => x.id);
                    table.ForeignKey(
                        name: "FK_LogConcorrenteProdutos_ConcorrenteProdutos_CodigoProdutoConc~",
                        column: x => x.CodigoProdutoConcorrenteId,
                        principalTable: "ConcorrenteProdutos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogConcorrenteProdutos_Concorrentes_ConcorrenteId",
                        column: x => x.ConcorrenteId,
                        principalTable: "Concorrentes",
                        principalColumn: "conId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogConcorrenteProdutos_Produtos_CodigoProdutoAntCodigoProduto",
                        column: x => x.CodigoProdutoAntCodigoProduto,
                        principalTable: "Produtos",
                        principalColumn: "codProd");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Nfis",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    ConcorrenteProdutoId = table.Column<string>(type: "varchar(255)", nullable: false),
                    NfcChaveNfe = table.Column<string>(type: "varchar(255)", nullable: true),
                    qtde = table.Column<int>(type: "int", nullable: false),
                    valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ufOrigin = table.Column<string>(type: "longtext", nullable: true),
                    ufDestino = table.Column<string>(type: "longtext", nullable: true),
                    codBarra = table.Column<string>(type: "longtext", nullable: true),
                    unidade = table.Column<string>(type: "longtext", nullable: true),
                    dtCadastro = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    usuCadastro = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nfis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nfis_ConcorrenteProdutos_ConcorrenteProdutoId",
                        column: x => x.ConcorrenteProdutoId,
                        principalTable: "ConcorrenteProdutos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nfis_Nfcs_NfcChaveNfe",
                        column: x => x.NfcChaveNfe,
                        principalTable: "Nfcs",
                        principalColumn: "chaveNfe");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7d4f6ea4-7d82-4f27-8cd0-73e7a4ee0d0b", null, "Admin", "Admin" },
                    { "9c2648cc-c643-4b43-abce-1958d666b1bb", null, "RCA", "RCA" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConcorrenteFilialPendentes_ConcorrenteId",
                table: "ConcorrenteFilialPendentes",
                column: "ConcorrenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcorrenteFilialPendentes_PlanilhaId",
                table: "ConcorrenteFilialPendentes",
                column: "PlanilhaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcorrenteFilials_ConcorrenteId",
                table: "ConcorrenteFilials",
                column: "ConcorrenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcorrenteFilialTemps_PlanilhaId",
                table: "ConcorrenteFilialTemps",
                column: "PlanilhaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcorrenteProdutos_ConcorrenteId",
                table: "ConcorrenteProdutos",
                column: "ConcorrenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcorrenteProdutos_PlanilhaId",
                table: "ConcorrenteProdutos",
                column: "PlanilhaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcorrenteProdutos_ProdutoCodigoProduto",
                table: "ConcorrenteProdutos",
                column: "ProdutoCodigoProduto");

            migrationBuilder.CreateIndex(
                name: "IX_LogConcorrenteProdutos_CodigoProdutoAntCodigoProduto",
                table: "LogConcorrenteProdutos",
                column: "CodigoProdutoAntCodigoProduto");

            migrationBuilder.CreateIndex(
                name: "IX_LogConcorrenteProdutos_CodigoProdutoConcorrenteId",
                table: "LogConcorrenteProdutos",
                column: "CodigoProdutoConcorrenteId");

            migrationBuilder.CreateIndex(
                name: "IX_LogConcorrenteProdutos_ConcorrenteId",
                table: "LogConcorrenteProdutos",
                column: "ConcorrenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Nfcs_ConcorrenteCnpjCnpj",
                table: "Nfcs",
                column: "ConcorrenteCnpjCnpj");

            migrationBuilder.CreateIndex(
                name: "IX_Nfcs_PlanilhaId",
                table: "Nfcs",
                column: "PlanilhaId");

            migrationBuilder.CreateIndex(
                name: "IX_Nfis_ConcorrenteProdutoId",
                table: "Nfis",
                column: "ConcorrenteProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Nfis_NfcChaveNfe",
                table: "Nfis",
                column: "NfcChaveNfe");

            migrationBuilder.CreateIndex(
                name: "IX_PlanilhaStatuses_PlanhilaId",
                table: "PlanilhaStatuses",
                column: "PlanhilaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ConcorrenteFilialPendentes");

            migrationBuilder.DropTable(
                name: "ConcorrenteFilialTemps");

            migrationBuilder.DropTable(
                name: "LogConcorrenteProdutos");

            migrationBuilder.DropTable(
                name: "Nfis");

            migrationBuilder.DropTable(
                name: "PlanilhaStatuses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ConcorrenteProdutos");

            migrationBuilder.DropTable(
                name: "Nfcs");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "ConcorrenteFilials");

            migrationBuilder.DropTable(
                name: "Planilhas");

            migrationBuilder.DropTable(
                name: "Concorrentes");
        }
    }
}
