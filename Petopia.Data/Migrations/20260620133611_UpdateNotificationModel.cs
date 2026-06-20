using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GoalId",
                table: "Notification",
                newName: "ReferenceId");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceType",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceType",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "ReferenceId",
                table: "Notification",
                newName: "GoalId");
        }
    }
}
