using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUpgradeFormUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UpgradeForm_UserId",
                table: "UpgradeForm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UpgradeForm_UserId",
                table: "UpgradeForm",
                column: "UserId",
                unique: true);
        }
    }
}
