using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWebProjFinal.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImageManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Media",
                table: "Paginas");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Utentes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Paginas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Utentes");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Paginas");

            migrationBuilder.AddColumn<string>(
                name: "Media",
                table: "Paginas",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
