using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuscaBrete.Migrations
{
    /// <inheritdoc />
    public partial class MigracionCurriculum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PerfilPostulante_PostulanteId",
                table: "PerfilPostulante");

            migrationBuilder.AlterColumn<string>(
                name: "PostulanteId",
                table: "Postulacion",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CvPath",
                table: "Postulacion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mensaje",
                table: "Postulacion",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostulanteId",
                table: "PerfilPostulante",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Habilidades",
                table: "PerfilPostulante",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "CVPath",
                table: "PerfilPostulante",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Postulacion_OfertaId",
                table: "Postulacion",
                column: "OfertaId");

            migrationBuilder.CreateIndex(
                name: "IX_Postulacion_PostulanteId",
                table: "Postulacion",
                column: "PostulanteId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPostulante_PostulanteId",
                table: "PerfilPostulante",
                column: "PostulanteId",
                unique: true,
                filter: "[PostulanteId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Postulacion_AspNetUsers_PostulanteId",
                table: "Postulacion",
                column: "PostulanteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Postulacion_Oferta_OfertaId",
                table: "Postulacion",
                column: "OfertaId",
                principalTable: "Oferta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Postulacion_AspNetUsers_PostulanteId",
                table: "Postulacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Postulacion_Oferta_OfertaId",
                table: "Postulacion");

            migrationBuilder.DropIndex(
                name: "IX_Postulacion_OfertaId",
                table: "Postulacion");

            migrationBuilder.DropIndex(
                name: "IX_Postulacion_PostulanteId",
                table: "Postulacion");

            migrationBuilder.DropIndex(
                name: "IX_PerfilPostulante_PostulanteId",
                table: "PerfilPostulante");

            migrationBuilder.DropColumn(
                name: "CvPath",
                table: "Postulacion");

            migrationBuilder.DropColumn(
                name: "Mensaje",
                table: "Postulacion");

            migrationBuilder.AlterColumn<string>(
                name: "PostulanteId",
                table: "Postulacion",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "PostulanteId",
                table: "PerfilPostulante",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Habilidades",
                table: "PerfilPostulante",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CVPath",
                table: "PerfilPostulante",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPostulante_PostulanteId",
                table: "PerfilPostulante",
                column: "PostulanteId",
                unique: true);
        }
    }
}
