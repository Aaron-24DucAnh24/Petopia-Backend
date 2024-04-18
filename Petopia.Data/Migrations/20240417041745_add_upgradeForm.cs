using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_upgradeForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WebSite",
                table: "UserOrganizationAttributes",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "OrganizationType",
                table: "UserOrganizationAttributes",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserOrganizationAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntityName",
                table: "UserOrganizationAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxCode",
                table: "UserOrganizationAttributes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserOrganizationAttributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UpgradeForm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivinceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WardCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpgradeForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpgradeForm_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpgradeForm");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserOrganizationAttributes");

            migrationBuilder.DropColumn(
                name: "EntityName",
                table: "UserOrganizationAttributes");

            migrationBuilder.DropColumn(
                name: "TaxCode",
                table: "UserOrganizationAttributes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserOrganizationAttributes");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "UserOrganizationAttributes",
                newName: "WebSite");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "UserOrganizationAttributes",
                newName: "OrganizationType");
        }
    }
}
