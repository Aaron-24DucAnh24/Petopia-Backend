using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncDataCollections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Action = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncDataCollections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: ""),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: ""),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsCreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ResetPasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordTokenExpirationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsDeactivated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Breed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    Species = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    IsVaccinated = table.Column<int>(type: "int", nullable: false),
                    IsSterillized = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsCreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsUpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    View = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pet_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserConnection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessTokenExpirationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RefreshTokenExpirationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSynced = table.Column<bool>(type: "bit", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medias_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "Body", "Subject", "Type" },
                values: new object[,]
                {
                    { new Guid("8ef178e4-2fc4-4592-be4f-38c568d5a6dd"), "<body> <div style=\" width: 100%; text-align: center; \"> <div style=\" background-color: #012979; height: 40px; line-height: 40px; font-size: 24px; font-style: italic; width: 100%; text-align: center; font-weight: 600; color: white; \"> Petopia </div> <div style=\" margin-top: 28px; font-size: 22px; margin-bottom: 28px; font-weight: 500; \">Chào mừng đến với nền tảng nhận nuôi thú cưng Petopia</div> <a style=\" background-color: #1662dd; text-decoration: none; font-size: 16px; color: white; font-weight: 500; padding: 16px; border-radius: 4px; margin-bottom: 16px; \" href=\"{foRoute}/register/validate?email={email}&validateRegisterToken={registerToken}\"> Nhấn để hoàn thành đăng ký </a> <div style=\"color: #9e9ea7; margin-top: 24px;\">Email xác thực có hiệu lực 30 phút.</div> </div> </body>", "XÁC THỰC EMAIL CỦA BẠN", 0 },
                    { new Guid("a96198b5-d868-45e7-8797-5ec7077a03cc"), "<body> <div style=\" width: 100%; text-align: center; \"> <div style=\" background-color: #012979; height: 40px; line-height: 40px; font-size: 24px; font-style: italic; width: 100%; text-align: center; font-weight: 600; color: white; \"> Petopia </div> <div style=\" margin-top: 28px; font-size: 22px; margin-bottom: 28px; font-weight: 500; \">Chào mừng đến với nền tảng nhận nuôi thú cưng Petopia</div> <a style=\" background-color: #1662dd; text-decoration: none; font-size: 16px; color: white; font-weight: 500; padding: 16px; border-radius: 4px; margin-bottom: 16px; \" href=\"{foRoute}/login/reset-password?passwordToken={passwordToken}\"> Nhấn để khôi phục mật khẩu </a> <div style=\"color: #9e9ea7; margin-top: 24px;\">Email xác thực có hiệu lực 1 giờ.</div> </div> </body>", "KHÔI PHỤC MẬT KHẨU CỦA BẠN", 1 }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "IsCreatedAt", "IsDeactivated", "Password", "ResetPasswordToken", "ResetPasswordTokenExpirationDate", "Role" },
                values: new object[] { new Guid("9ec23313-ac63-4dcd-b890-b9657e7179a3"), "6154B051E9E3D3179DE74C08D3294698CBEA931503E638A77CE9F43FB25E2710", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "$2a$11$ZPqHXEjL.1K8ZYbRbY/jbOElxxjGpB34lyBdtzVvDVjvy2SvarzMq", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "UserIndividualAttributes",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("9ec23313-ac63-4dcd-b890-b9657e7179a3"), "Đức Anh", "Bùi" });

            migrationBuilder.CreateIndex(
                name: "IX_Medias_PetId",
                table: "Medias",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_OwnerId",
                table: "Pet",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropTable(
                name: "SyncDataCollections");

            migrationBuilder.DropTable(
                name: "UserConnection");

            migrationBuilder.DropTable(
                name: "UserIndividualAttributes");

            migrationBuilder.DropTable(
                name: "UserOrganizationAttributes");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
