using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class AgregarVentaIdEnValoracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VentaId",
                table: "Valoraciones",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Docentes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Apellido = table.Column<string>(type: "text", nullable: true),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Correo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docentes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Fecha_inicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Id_docente = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cursos_Docentes_Id_docente",
                        column: x => x.Id_docente,
                        principalTable: "Docentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Valoraciones_VentaId",
                table: "Valoraciones",
                column: "VentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cursos_Id_docente",
                table: "Cursos",
                column: "Id_docente");

            migrationBuilder.AddForeignKey(
                name: "FK_Valoraciones_Ventas_VentaId",
                table: "Valoraciones",
                column: "VentaId",
                principalTable: "Ventas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Valoraciones_Ventas_VentaId",
                table: "Valoraciones");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Docentes");

            migrationBuilder.DropIndex(
                name: "IX_Valoraciones_VentaId",
                table: "Valoraciones");

            migrationBuilder.DropColumn(
                name: "VentaId",
                table: "Valoraciones");
        }
    }
}
