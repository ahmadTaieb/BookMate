using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookMate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addposts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("1105173d-e9b8-44b6-8fd5-ace5315f9acb"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("9f216443-74b3-4bd3-8ac7-98308a1236d2"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("d737f8c8-57b3-4f2b-b2c9-bd726acfd3cb"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("dd86c956-4af6-4e91-9657-ce6f0120eca0"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f667f0d5-0a7f-40a4-8f00-c55a80345cc1"));

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posts_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reactes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Reaction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactes_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reactes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AverageRating", "Description", "ImageUrl", "NumberOfPages", "PdfUrl", "PublishedYear", "RatingsCount", "ReadingCount", "Title", "VoiceUrl" },
                values: new object[,]
                {
                    { new Guid("0e22555e-882c-48ee-bd41-a5e8448aad1e"), "Author5", null, null, null, 500, null, null, null, null, "Test5", null },
                    { new Guid("22383e34-0d46-4684-b977-bc6835d5dbcd"), "Author1", null, null, null, 100, null, null, null, null, "Test1", null },
                    { new Guid("6f24656a-0497-418d-8444-1931d78af8a3"), "Author3", null, null, null, 300, null, null, null, null, "Test3", null },
                    { new Guid("851231cd-f988-45f1-99e0-afa277a37339"), "Author4", null, null, null, 400, null, null, null, null, "Test4", null },
                    { new Guid("b9b9755b-6f70-493d-9e9f-d7215ab7924e"), "Author2", null, null, null, 200, null, null, null, null, "Test2", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApplicationUserId",
                table: "Comments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ApplicationUserId",
                table: "Posts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ClubId",
                table: "Posts",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactes_ApplicationUserId",
                table: "Reactes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactes_PostId",
                table: "Reactes",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Reactes");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("0e22555e-882c-48ee-bd41-a5e8448aad1e"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("22383e34-0d46-4684-b977-bc6835d5dbcd"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("6f24656a-0497-418d-8444-1931d78af8a3"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("851231cd-f988-45f1-99e0-afa277a37339"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b9b9755b-6f70-493d-9e9f-d7215ab7924e"));

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AverageRating", "Description", "ImageUrl", "NumberOfPages", "PdfUrl", "PublishedYear", "RatingsCount", "ReadingCount", "Title", "VoiceUrl" },
                values: new object[,]
                {
                    { new Guid("1105173d-e9b8-44b6-8fd5-ace5315f9acb"), "Author1", null, null, null, 100, null, null, null, null, "Test1", null },
                    { new Guid("9f216443-74b3-4bd3-8ac7-98308a1236d2"), "Author5", null, null, null, 500, null, null, null, null, "Test5", null },
                    { new Guid("d737f8c8-57b3-4f2b-b2c9-bd726acfd3cb"), "Author3", null, null, null, 300, null, null, null, null, "Test3", null },
                    { new Guid("dd86c956-4af6-4e91-9657-ce6f0120eca0"), "Author2", null, null, null, 200, null, null, null, null, "Test2", null },
                    { new Guid("f667f0d5-0a7f-40a4-8f00-c55a80345cc1"), "Author4", null, null, null, 400, null, null, null, null, "Test4", null }
                });
        }
    }
}
