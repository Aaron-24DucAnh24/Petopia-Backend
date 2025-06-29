using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_post_remove_adoption_form : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Pet_PetId",
                table: "Post");

            migrationBuilder.DropTable(
                name: "AdoptionForm");

            migrationBuilder.RenameColumn(
                name: "PetId",
                table: "Post",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_PetId",
                table: "Post",
                newName: "IX_Post_UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Post",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User_UserId",
                table: "Post",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_User_UserId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Post",
                newName: "PetId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_UserId",
                table: "Post",
                newName: "IX_Post_PetId");

            migrationBuilder.CreateTable(
                name: "AdoptionForm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdopterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DelayDuration = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    HouseType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsCreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsOwnerBefore = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsSeen = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsSeenByAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsUpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdoptionForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdoptionForm_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdoptionForm_User_AdopterId",
                        column: x => x.AdopterId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionForm_AdopterId",
                table: "AdoptionForm",
                column: "AdopterId");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionForm_PetId",
                table: "AdoptionForm",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Pet_PetId",
                table: "Post",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id");
        }
    }
}
