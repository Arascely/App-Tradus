using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradusApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChapterVersionAndReportsAndIdioma : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Idioma",
                table: "Libros",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VersionActual",
                table: "Capitulos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ChapterVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CapituloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterVersions_Capitulos_CapituloId",
                        column: x => x.CapituloId,
                        principalTable: "Capitulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CapituloId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Capitulos_CapituloId",
                        column: x => x.CapituloId,
                        principalTable: "Capitulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Autor",
                table: "Libros",
                column: "Autor");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Idioma",
                table: "Libros",
                column: "Idioma");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_Titulo",
                table: "Libros",
                column: "Titulo");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterVersions_CapituloId_Version",
                table: "ChapterVersions",
                columns: new[] { "CapituloId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CapituloId",
                table: "Reports",
                column: "CapituloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterVersions");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Libros_Autor",
                table: "Libros");

            migrationBuilder.DropIndex(
                name: "IX_Libros_Idioma",
                table: "Libros");

            migrationBuilder.DropIndex(
                name: "IX_Libros_Titulo",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "Idioma",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "VersionActual",
                table: "Capitulos");
        }
    }
}
