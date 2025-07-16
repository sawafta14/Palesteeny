using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Palesteeny_Project.Migrations
{
    /// <inheritdoc />
    public partial class FixA23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreferredAssistantId",
                table: "UsersPal",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersPal_PreferredAssistantId",
                table: "UsersPal",
                column: "PreferredAssistantId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersPal_AIAssistant_PreferredAssistantId",
                table: "UsersPal",
                column: "PreferredAssistantId",
                principalTable: "AIAssistant",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersPal_AIAssistant_PreferredAssistantId",
                table: "UsersPal");

            migrationBuilder.DropIndex(
                name: "IX_UsersPal_PreferredAssistantId",
                table: "UsersPal");

            migrationBuilder.DropColumn(
                name: "PreferredAssistantId",
                table: "UsersPal");
        }
    }
}
