using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Palesteeny_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStoryRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Qusis_UsersPal_UserPalId",
                table: "Qusis");

            migrationBuilder.DropIndex(
                name: "IX_Qusis_UserPalId",
                table: "Qusis");

            migrationBuilder.DropColumn(
                name: "UserPalId",
                table: "Qusis");

            migrationBuilder.CreateTable(
                name: "FavoriteStories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPalId = table.Column<int>(type: "int", nullable: false),
                    QusiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteStories_Qusis_QusiId",
                        column: x => x.QusiId,
                        principalTable: "Qusis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteStories_UsersPal_UserPalId",
                        column: x => x.UserPalId,
                        principalTable: "UsersPal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryBookmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPalId = table.Column<int>(type: "int", nullable: false),
                    QusiId = table.Column<int>(type: "int", nullable: false),
                    LastImageIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryBookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryBookmarks_Qusis_QusiId",
                        column: x => x.QusiId,
                        principalTable: "Qusis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoryBookmarks_UsersPal_UserPalId",
                        column: x => x.UserPalId,
                        principalTable: "UsersPal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteStories_QusiId",
                table: "FavoriteStories",
                column: "QusiId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteStories_UserPalId",
                table: "FavoriteStories",
                column: "UserPalId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryBookmarks_QusiId",
                table: "StoryBookmarks",
                column: "QusiId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryBookmarks_UserPalId",
                table: "StoryBookmarks",
                column: "UserPalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteStories");

            migrationBuilder.DropTable(
                name: "StoryBookmarks");

            migrationBuilder.AddColumn<int>(
                name: "UserPalId",
                table: "Qusis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Qusis_UserPalId",
                table: "Qusis",
                column: "UserPalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Qusis_UsersPal_UserPalId",
                table: "Qusis",
                column: "UserPalId",
                principalTable: "UsersPal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
