using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProductInfo_ProductInfos_ProductsProductId",
                table: "OrderProductInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInfos",
                table: "ProductInfos");

            migrationBuilder.RenameColumn(
                name: "ProductsProductId",
                table: "OrderProductInfo",
                newName: "ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderProductInfo_ProductsProductId",
                table: "OrderProductInfo",
                newName: "IX_OrderProductInfo_ProductsId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductInfos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInfos",
                table: "ProductInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProductInfo_ProductInfos_ProductsId",
                table: "OrderProductInfo",
                column: "ProductsId",
                principalTable: "ProductInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProductInfo_ProductInfos_ProductsId",
                table: "OrderProductInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInfos",
                table: "ProductInfos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductInfos");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "OrderProductInfo",
                newName: "ProductsProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderProductInfo_ProductsId",
                table: "OrderProductInfo",
                newName: "IX_OrderProductInfo_ProductsProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInfos",
                table: "ProductInfos",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProductInfo_ProductInfos_ProductsProductId",
                table: "OrderProductInfo",
                column: "ProductsProductId",
                principalTable: "ProductInfos",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
