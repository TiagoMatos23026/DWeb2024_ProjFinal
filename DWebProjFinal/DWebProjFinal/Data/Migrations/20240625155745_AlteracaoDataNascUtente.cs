using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWebProjFinal.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlteracaoDataNascUtente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "dataNasc",
                table: "Utentes",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "dataNasc",
                table: "Utentes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
