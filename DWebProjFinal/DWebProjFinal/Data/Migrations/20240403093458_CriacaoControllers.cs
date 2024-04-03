using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWebProjFinal.Data.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoControllers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dataNasc = table.Column<DateOnly>(type: "date", nullable: false),
                    Biografia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utentes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paginas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dificuldade = table.Column<int>(type: "int", nullable: false),
                    Media = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtenteFK = table.Column<int>(type: "int", nullable: false),
                    UtentesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paginas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paginas_Utentes_UtentesId",
                        column: x => x.UtentesId,
                        principalTable: "Utentes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriasId = table.Column<int>(type: "int", nullable: true),
                    PaginasId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_Categorias_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "Categorias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Categorias_Paginas_PaginasId",
                        column: x => x.PaginasId,
                        principalTable: "Paginas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CategoriasId",
                table: "Categorias",
                column: "CategoriasId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_PaginasId",
                table: "Categorias",
                column: "PaginasId");

            migrationBuilder.CreateIndex(
                name: "IX_Paginas_UtentesId",
                table: "Paginas",
                column: "UtentesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Paginas");

            migrationBuilder.DropTable(
                name: "Utentes");
        }
    }
}
