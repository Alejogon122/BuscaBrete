using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuscaBrete.Migrations
{
    /// <inheritdoc />
    public partial class AddNombreEmpresaToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Educacion",
                table: "PerfilPostulante");

            migrationBuilder.DropColumn(
                name: "Experiencia",
                table: "PerfilPostulante");

            migrationBuilder.AlterColumn<string>(
                name: "PostulanteId",
                table: "PerfilPostulante",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "PerfilPostulante",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "PerfilPostulante",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Habilidades",
                table: "PerfilPostulante",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Resumen",
                table: "PerfilPostulante",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "PerfilPostulante",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmpresaId",
                table: "Oferta",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NombreEmpresa",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Educacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfilPostulanteId = table.Column<int>(type: "int", nullable: false),
                    Institucion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educacion_PerfilPostulante_PerfilPostulanteId",
                        column: x => x.PerfilPostulanteId,
                        principalTable: "PerfilPostulante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperienciaLaboral",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfilPostulanteId = table.Column<int>(type: "int", nullable: false),
                    Puesto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Empresa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Actualmente = table.Column<bool>(type: "bit", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienciaLaboral", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienciaLaboral_PerfilPostulante_PerfilPostulanteId",
                        column: x => x.PerfilPostulanteId,
                        principalTable: "PerfilPostulante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPostulante_PostulanteId",
                table: "PerfilPostulante",
                column: "PostulanteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Oferta_EmpresaId",
                table: "Oferta",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Educacion_PerfilPostulanteId",
                table: "Educacion",
                column: "PerfilPostulanteId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienciaLaboral_PerfilPostulanteId",
                table: "ExperienciaLaboral",
                column: "PerfilPostulanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Oferta_AspNetUsers_EmpresaId",
                table: "Oferta",
                column: "EmpresaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PerfilPostulante_AspNetUsers_PostulanteId",
                table: "PerfilPostulante",
                column: "PostulanteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oferta_AspNetUsers_EmpresaId",
                table: "Oferta");

            migrationBuilder.DropForeignKey(
                name: "FK_PerfilPostulante_AspNetUsers_PostulanteId",
                table: "PerfilPostulante");

            migrationBuilder.DropTable(
                name: "Educacion");

            migrationBuilder.DropTable(
                name: "ExperienciaLaboral");

            migrationBuilder.DropIndex(
                name: "IX_PerfilPostulante_PostulanteId",
                table: "PerfilPostulante");

            migrationBuilder.DropIndex(
                name: "IX_Oferta_EmpresaId",
                table: "Oferta");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "PerfilPostulante");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "PerfilPostulante");

            migrationBuilder.DropColumn(
                name: "Habilidades",
                table: "PerfilPostulante");

            migrationBuilder.DropColumn(
                name: "Resumen",
                table: "PerfilPostulante");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "PerfilPostulante");

            migrationBuilder.DropColumn(
                name: "NombreEmpresa",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PostulanteId",
                table: "PerfilPostulante",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Educacion",
                table: "PerfilPostulante",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Experiencia",
                table: "PerfilPostulante",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "EmpresaId",
                table: "Oferta",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
