using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_upgradeForm_add_userId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UpgradeForm_User_Id",
                table: "UpgradeForm");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UpgradeForm",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UpgradeForm_UserId",
                table: "UpgradeForm",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UpgradeForm_User_UserId",
                table: "UpgradeForm",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UpgradeForm_User_UserId",
                table: "UpgradeForm");

            migrationBuilder.DropIndex(
                name: "IX_UpgradeForm_UserId",
                table: "UpgradeForm");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UpgradeForm");

            migrationBuilder.AddForeignKey(
                name: "FK_UpgradeForm_User_Id",
                table: "UpgradeForm",
                column: "Id",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
