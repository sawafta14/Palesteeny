using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Palesteeny_Project.Migrations
{
    /// <inheritdoc />
    public partial class NewItemSS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Semester",
                table: "UsersPal",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Semester",
                table: "UsersPal");
        }
    }
}
