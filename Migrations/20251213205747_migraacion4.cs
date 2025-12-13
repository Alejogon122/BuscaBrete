using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuscaBrete.Migrations
{
    /// <inheritdoc />
    public partial class migraacion4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvPath",
                table: "Postulacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvPath",
                table: "Postulacion",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
