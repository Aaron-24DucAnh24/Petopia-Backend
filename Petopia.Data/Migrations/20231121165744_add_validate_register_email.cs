using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_validate_register_email : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserIndividualAttributes",
                keyColumn: "Id",
                keyValue: new Guid("bc9085f0-2816-44e4-a1d2-eb2050ab301d"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("bc9085f0-2816-44e4-a1d2-eb2050ab301d"));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RefreshTokenExpirationDate",
                table: "UserConnection",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 21, 23, 57, 44, 695, DateTimeKind.Unspecified).AddTicks(4560), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Unspecified).AddTicks(7830), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "AccessTokenExpirationDate",
                table: "UserConnection",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 21, 23, 57, 44, 695, DateTimeKind.Unspecified).AddTicks(4270), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Unspecified).AddTicks(7660), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTime>(
                name: "IsCreatedAt",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 21, 23, 57, 44, 694, DateTimeKind.Local).AddTicks(6140),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Local).AddTicks(2640));

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    EmailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.EmailId);
                });

            migrationBuilder.InsertData(
                table: "Email",
                columns: new[] { "EmailId", "Body", "Subject", "Type" },
                values: new object[] { new Guid("ba8bb0a2-298c-4425-91b4-b2c6eedb7f8d"), "<body> <div style=\" width: 100%; text-align: center; \"> <div style=\" background-color: #012979; height: 40px; line-height: 40px; font-size: 24px; font-style: italic; width: 100%; text-align: center; font-weight: 600; color: white; \"> Petopia </div> <div style=\" margin-top: 28px; font-size: 22px; margin-bottom: 28px; font-weight: 500; \">Chào mừng đến với nền tảng nhận nuôi thú cưng Petopia</div> <a style=\" background-color: #1662dd; text-decoration: none; font-size: 16px; color: white; font-weight: 500; padding: 16px; border-radius: 4px; margin-bottom: 16px; \" href=\"{apiRoute}/api/Authentication/ValidateRegister?Email={email}&ValidateRegisterToken={registerToken}\"> Nhấn để hoàn thành đăng ký </a> <div style=\"color: #9e9ea7; margin-top: 24px;\">Email xác thực có hiệu lực 30 phút.</div> </div> </body>", "XÁC THỰC EMAIL CỦA BẠN", 0 });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Password", "ResetPasswordToken", "ResetPasswordTokenExpirationDate", "Role" },
                values: new object[] { new Guid("d492a242-7612-4408-a5b0-09d30defff8e"), "6154B051E9E3D3179DE74C08D3294698CBEA931503E638A77CE9F43FB25E2710", "$2a$11$ZPqHXEjL.1K8ZYbRbY/jbOElxxjGpB34lyBdtzVvDVjvy2SvarzMq", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "UserIndividualAttributes",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("d492a242-7612-4408-a5b0-09d30defff8e"), "Đức Anh", "Bùi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DeleteData(
                table: "UserIndividualAttributes",
                keyColumn: "Id",
                keyValue: new Guid("d492a242-7612-4408-a5b0-09d30defff8e"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("d492a242-7612-4408-a5b0-09d30defff8e"));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RefreshTokenExpirationDate",
                table: "UserConnection",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Unspecified).AddTicks(7830), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 21, 23, 57, 44, 695, DateTimeKind.Unspecified).AddTicks(4560), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "AccessTokenExpirationDate",
                table: "UserConnection",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Unspecified).AddTicks(7660), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 21, 23, 57, 44, 695, DateTimeKind.Unspecified).AddTicks(4270), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTime>(
                name: "IsCreatedAt",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Local).AddTicks(2640),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 21, 23, 57, 44, 694, DateTimeKind.Local).AddTicks(6140));

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Password", "ResetPasswordToken", "ResetPasswordTokenExpirationDate", "Role" },
                values: new object[] { new Guid("bc9085f0-2816-44e4-a1d2-eb2050ab301d"), "6154B051E9E3D3179DE74C08D3294698CBEA931503E638A77CE9F43FB25E2710", "$2a$11$ZPqHXEjL.1K8ZYbRbY/jbOElxxjGpB34lyBdtzVvDVjvy2SvarzMq", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "UserIndividualAttributes",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("bc9085f0-2816-44e4-a1d2-eb2050ab301d"), "Đức Anh", "Bùi" });
        }
    }
}
