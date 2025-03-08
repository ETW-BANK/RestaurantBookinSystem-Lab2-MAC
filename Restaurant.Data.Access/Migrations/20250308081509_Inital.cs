using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menues_Categories_menuId",
                table: "Menues");

            migrationBuilder.DropIndex(
                name: "IX_Menues_menuId",
                table: "Menues");

            migrationBuilder.DropColumn(
                name: "menuId",
                table: "Menues");

            migrationBuilder.DropColumn(
                name: "menuId",
                table: "Categories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "menuId",
                table: "Menues",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "menuId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Menues_menuId",
                table: "Menues",
                column: "menuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menues_Categories_menuId",
                table: "Menues",
                column: "menuId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
