using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Booklist.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UserID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "lastName");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "Users",
                newName: "username");

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "Books",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserID",
                table: "Books",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UserID",
                table: "Books",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
