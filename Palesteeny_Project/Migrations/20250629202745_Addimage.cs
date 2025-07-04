using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Palesteeny_Project.Migrations
{
    /// <inheritdoc />
    public partial class Addimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QusiImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QusiId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QusiImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QusiImages_Qusis_QusiId",
                        column: x => x.QusiId,
                        principalTable: "Qusis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QusiImages_QusiId",
                table: "QusiImages",
                column: "QusiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QusiImages");
        }
    }
}
