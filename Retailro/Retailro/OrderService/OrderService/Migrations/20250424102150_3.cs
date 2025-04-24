using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProductInfo");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "ProductInfos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductInfos_OrderId",
                table: "ProductInfos",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInfos_Orders_OrderId",
                table: "ProductInfos",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInfos_Orders_OrderId",
                table: "ProductInfos");

            migrationBuilder.DropIndex(
                name: "IX_ProductInfos_OrderId",
                table: "ProductInfos");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ProductInfos");

            migrationBuilder.CreateTable(
                name: "OrderProductInfo",
                columns: table => new
                {
                    OrdersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProductInfo", x => new { x.OrdersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_OrderProductInfo_Orders_OrdersId",
                        column: x => x.OrdersId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProductInfo_ProductInfos_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "ProductInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductInfo_ProductsId",
                table: "OrderProductInfo",
                column: "ProductsId");
        }
    }
}
