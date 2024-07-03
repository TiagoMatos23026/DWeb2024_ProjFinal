using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWebProjFinal.Data.Migrations
{
    /// <inheritdoc />
    public partial class DisplayNameEmListaPaginas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paginas_Utentes_UtenteFK",
                table: "Paginas");

            migrationBuilder.AlterColumn<int>(
                name: "UtenteFK",
                table: "Paginas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Paginas_Utentes_UtenteFK",
                table: "Paginas",
                column: "UtenteFK",
                principalTable: "Utentes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paginas_Utentes_UtenteFK",
                table: "Paginas");

            migrationBuilder.AlterColumn<int>(
                name: "UtenteFK",
                table: "Paginas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Paginas_Utentes_UtenteFK",
                table: "Paginas",
                column: "UtenteFK",
                principalTable: "Utentes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
