using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWebProjFinal.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoModelosRelacionamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Categorias_CategoriasId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_Paginas_PaginasId",
                table: "Categorias");

            migrationBuilder.DropForeignKey(
                name: "FK_Paginas_Utentes_UtentesId",
                table: "Paginas");

            migrationBuilder.DropIndex(
                name: "IX_Paginas_UtentesId",
                table: "Paginas");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_CategoriasId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_PaginasId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "UtentesId",
                table: "Paginas");

            migrationBuilder.DropColumn(
                name: "CategoriasId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "PaginasId",
                table: "Categorias");

            migrationBuilder.CreateTable(
                name: "CategoriasPaginas",
                columns: table => new
                {
                    ListaCategoriasId = table.Column<int>(type: "int", nullable: false),
                    ListaPaginasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasPaginas", x => new { x.ListaCategoriasId, x.ListaPaginasId });
                    table.ForeignKey(
                        name: "FK_CategoriasPaginas_Categorias_ListaCategoriasId",
                        column: x => x.ListaCategoriasId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriasPaginas_Paginas_ListaPaginasId",
                        column: x => x.ListaPaginasId,
                        principalTable: "Paginas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paginas_UtenteFK",
                table: "Paginas",
                column: "UtenteFK");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriasPaginas_ListaPaginasId",
                table: "CategoriasPaginas",
                column: "ListaPaginasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paginas_Utentes_UtenteFK",
                table: "Paginas",
                column: "UtenteFK",
                principalTable: "Utentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paginas_Utentes_UtenteFK",
                table: "Paginas");

            migrationBuilder.DropTable(
                name: "CategoriasPaginas");

            migrationBuilder.DropIndex(
                name: "IX_Paginas_UtenteFK",
                table: "Paginas");

            migrationBuilder.AddColumn<int>(
                name: "UtentesId",
                table: "Paginas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriasId",
                table: "Categorias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaginasId",
                table: "Categorias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paginas_UtentesId",
                table: "Paginas",
                column: "UtentesId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CategoriasId",
                table: "Categorias",
                column: "CategoriasId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_PaginasId",
                table: "Categorias",
                column: "PaginasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Categorias_CategoriasId",
                table: "Categorias",
                column: "CategoriasId",
                principalTable: "Categorias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_Paginas_PaginasId",
                table: "Categorias",
                column: "PaginasId",
                principalTable: "Paginas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Paginas_Utentes_UtentesId",
                table: "Paginas",
                column: "UtentesId",
                principalTable: "Utentes",
                principalColumn: "Id");
        }
    }
}
