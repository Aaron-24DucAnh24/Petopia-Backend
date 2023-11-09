using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_userConnection_add_token_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserIndividualAttributes",
                keyColumn: "Id",
                keyValue: new Guid("007295d5-4d83-441f-b545-1ee471c0f8cf"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("007295d5-4d83-441f-b545-1ee471c0f8cf"));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RefreshTokenExpirationDate",
                table: "UserConnection",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Unspecified).AddTicks(7830), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 6, 15, 46, 56, 491, DateTimeKind.Unspecified).AddTicks(1228), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "AccessTokenExpirationDate",
                table: "UserConnection",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Unspecified).AddTicks(7660), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 6, 15, 46, 56, 491, DateTimeKind.Unspecified).AddTicks(546), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "UserConnection",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "UserConnection",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "IsCreatedAt",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Local).AddTicks(2640),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 6, 15, 46, 56, 489, DateTimeKind.Local).AddTicks(3865));

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Password", "ResetPasswordToken", "ResetPasswordTokenExpirationDate", "Role" },
                values: new object[] { new Guid("bc9085f0-2816-44e4-a1d2-eb2050ab301d"), "6154B051E9E3D3179DE74C08D3294698CBEA931503E638A77CE9F43FB25E2710", "$2a$11$ZPqHXEjL.1K8ZYbRbY/jbOElxxjGpB34lyBdtzVvDVjvy2SvarzMq", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "UserIndividualAttributes",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("bc9085f0-2816-44e4-a1d2-eb2050ab301d"), "Đức Anh", "Bùi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserIndividualAttributes",
                keyColumn: "Id",
                keyValue: new Guid("bc9085f0-2816-44e4-a1d2-eb2050ab301d"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("bc9085f0-2816-44e4-a1d2-eb2050ab301d"));

            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "UserConnection");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "UserConnection");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "RefreshTokenExpirationDate",
                table: "UserConnection",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 6, 15, 46, 56, 491, DateTimeKind.Unspecified).AddTicks(1228), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Unspecified).AddTicks(7830), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "AccessTokenExpirationDate",
                table: "UserConnection",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 11, 6, 15, 46, 56, 491, DateTimeKind.Unspecified).AddTicks(546), new TimeSpan(0, 7, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Unspecified).AddTicks(7660), new TimeSpan(0, 7, 0, 0, 0)));

            migrationBuilder.AlterColumn<DateTime>(
                name: "IsCreatedAt",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 11, 6, 15, 46, 56, 489, DateTimeKind.Local).AddTicks(3865),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 11, 10, 0, 13, 52, 748, DateTimeKind.Local).AddTicks(2640));

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "Password", "ResetPasswordToken", "ResetPasswordTokenExpirationDate", "Role" },
                values: new object[] { new Guid("007295d5-4d83-441f-b545-1ee471c0f8cf"), "6154B051E9E3D3179DE74C08D3294698CBEA931503E638A77CE9F43FB25E2710", "$2a$11$ZPqHXEjL.1K8ZYbRbY/jbOElxxjGpB34lyBdtzVvDVjvy2SvarzMq", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 1 });

            migrationBuilder.InsertData(
                table: "UserIndividualAttributes",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[] { new Guid("007295d5-4d83-441f-b545-1ee471c0f8cf"), "Đức Anh", "Bùi" });
        }
    }
}
