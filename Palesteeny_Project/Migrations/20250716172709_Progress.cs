using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Palesteeny_Project.Migrations
{
    /// <inheritdoc />
    public partial class Progress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookProgresses",
                columns: table => new
                {
                    BookProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPalId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    ProgressPercent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookProgresses", x => x.BookProgressId);
                    table.ForeignKey(
                        name: "FK_BookProgresses_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookProgresses_UsersPal_UserPalId",
                        column: x => x.UserPalId,
                        principalTable: "UsersPal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookProgresses_BookId",
                table: "BookProgresses",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookProgresses_UserPalId",
                table: "BookProgresses",
                column: "UserPalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookProgresses");
        }
    }
}
