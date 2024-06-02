using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_vaccine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vaccine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetVaccine",
                columns: table => new
                {
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VaccineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetVaccine", x => new { x.PetId, x.VaccineId });
                    table.ForeignKey(
                        name: "FK_PetVaccine_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetVaccine_Vaccine_VaccineId",
                        column: x => x.VaccineId,
                        principalTable: "Vaccine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetVaccine_VaccineId",
                table: "PetVaccine",
                column: "VaccineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetVaccine");

            migrationBuilder.DropTable(
                name: "Vaccine");
        }
    }
}
