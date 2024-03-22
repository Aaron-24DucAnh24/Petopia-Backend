using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_pet_remove_address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Pet");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Pet",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
