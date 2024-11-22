using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyndMapper.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedCanvases",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Canvases_OwnerId",
                table: "Canvases",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Canvases_Users_OwnerId",
                table: "Canvases",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Canvases_Users_OwnerId",
                table: "Canvases");

            migrationBuilder.DropIndex(
                name: "IX_Canvases_OwnerId",
                table: "Canvases");

            migrationBuilder.AddColumn<string>(
                name: "CreatedCanvases",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
