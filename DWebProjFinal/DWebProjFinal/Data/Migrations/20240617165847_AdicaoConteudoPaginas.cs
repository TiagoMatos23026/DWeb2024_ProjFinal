using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWebProjFinal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoConteudoPaginas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Conteudo",
                table: "Paginas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Conteudo",
                table: "Paginas");
        }
    }
}
