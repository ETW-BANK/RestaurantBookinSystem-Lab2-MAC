using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant.Data.Access.Migrations
{
    /// <inheritdoc />
    public partial class changeinproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Menues_menueId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_menueId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "menueId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "menueId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_menueId",
                table: "Bookings",
                column: "menueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Menues_menueId",
                table: "Bookings",
                column: "menueId",
                principalTable: "Menues",
                principalColumn: "menueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
