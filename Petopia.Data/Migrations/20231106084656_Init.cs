using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SyncDataCollection",
                columns: table => new
                {
                    CollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Action = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncDataCollection", x => x.CollectionId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: ""),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 6, 15, 46, 56, 489, DateTimeKind.Local).AddTicks(3865)),
                    ResetPasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordTokenExpirationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserConnection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessTokenExpirationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 11, 6, 15, 46, 56, 491, DateTimeKind.Unspecified).AddTicks(546), new TimeSpan(0, 7, 0, 0, 0))),
                    RefreshTokenExpirationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2023, 11, 6, 15, 46, 56, 491, DateTimeKind.Unspecified).AddTicks(1228), new TimeSpan(0, 7, 0, 0, 0))),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserConnection_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserIndividualAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIndividualAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIndividualAttributes_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOrganizationAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrganizationAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOrganizationAttributes_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Password", "ResetPasswordToken", "ResetPasswordTokenExpirationDate", "Role" },
                values: new object[] { new Guid("007295d5-4d83-441f-b545-1ee471c0f8cf"), "6154B051E9E3D3179DE74C08D3294698CBEA931503E638A77CE9F43FB25E2710", "$2a$11$ZPqHXEjL.1K8ZYbRbY/jbOElxxjGpB34lyBdtzVvDVjvy2SvarzMq", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "UserIndividualAttributes",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("007295d5-4d83-441f-b545-1ee471c0f8cf"), "Đức Anh", "Bùi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyncDataCollection");

            migrationBuilder.DropTable(
                name: "UserConnection");

            migrationBuilder.DropTable(
                name: "UserIndividualAttributes");

            migrationBuilder.DropTable(
                name: "UserOrganizationAttributes");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
