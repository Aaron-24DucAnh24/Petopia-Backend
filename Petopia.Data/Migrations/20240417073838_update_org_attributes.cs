using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_org_attributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserOrganizationAttributes");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserOrganizationAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UpgradeForm",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserOrganizationAttributes");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UpgradeForm");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserOrganizationAttributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
