using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Burgermania.Migrations
{
    /// <inheritdoc />
    public partial class _9_OrderModelChangeJsonName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderId1",
                table: "Order",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Order_OrderId1",
                table: "Order",
                column: "OrderId1",
                principalTable: "Order",
                principalColumn: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Order_OrderId1",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderId1",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "Order");
        }
    }
}
