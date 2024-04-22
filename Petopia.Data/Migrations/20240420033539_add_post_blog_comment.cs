using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petopia.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_post_blog_comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			    migrationBuilder.CreateTable(
		        name: "Blog",
		        columns: table => new
		        {
			        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
			        UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
			        Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
			        Excerpt = table.Column<string>(type: "nvarchar(max)", nullable: true),
						  Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
			        Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
						  View = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
						  Like = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
						  IsHidden = table.Column<bool>(type: "bit", nullable: true),
						  Category = table.Column<int>(type: "int", nullable: false),
						  IsCreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
						  IsUpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
					  },
		        constraints: table =>
		        {
			        table.PrimaryKey("PK_Blog", x => x.Id);
			        table.ForeignKey(
							  name: "FK_Blog_User_UserId",
							  column: x => x.UserId,
							  principalTable: "User",
							  principalColumn: "Id",
							  onDelete: ReferentialAction.Cascade);
		        });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Like = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsCreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Blog_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

			      migrationBuilder.AddColumn<Guid>(
		          name: "PostId",
		          table: "Medias",
		          type: "uniqueidentifier",
		          nullable: true);

			      migrationBuilder.CreateIndex(
		          name: "IX_Blog_UserId",
		          table: "Blog",
		          column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_BlogId",
                table: "Comment",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_PetId",
                table: "Post",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Post_PostId",
                table: "Medias",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

			      migrationBuilder.CreateIndex(
		            name: "IX_Medias_PostId",
		            table: "Medias",
		            column: "PostId");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Post_PostId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Medias");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Post");

			      migrationBuilder.DropTable(
		            name: "Blog");

			      migrationBuilder.DropIndex(
                name: "IX_Medias_PostId",
                table: "Medias");

        }
    }
}
