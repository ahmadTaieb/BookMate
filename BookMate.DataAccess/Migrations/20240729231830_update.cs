using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookMate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactes_AspNetUsers_ApplicationUserId",
                table: "Reactes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactes_Posts_PostId",
                table: "Reactes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reactes",
                table: "Reactes");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("28b4208c-f594-430e-af57-41b109199b35"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("580214fb-113c-4c1b-884d-ca0911aaddbd"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("9b7a7e38-e810-40f1-9b2d-4d795f46cee5"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("ceae109a-7be8-4126-8b22-4ae210becb63"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e64b4582-0421-4b20-b811-421c4d9326b6"));

            migrationBuilder.RenameTable(
                name: "Reactes",
                newName: "Reacts");

            migrationBuilder.RenameIndex(
                name: "IX_Reactes_PostId",
                table: "Reacts",
                newName: "IX_Reacts_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Reactes_ApplicationUserId",
                table: "Reacts",
                newName: "IX_Reacts_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reacts",
                table: "Reacts",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AverageRating", "Description", "ImageUrl", "NumberOfPages", "PdfUrl", "PublishedYear", "RatingsCount", "ReadingCount", "Title", "VoiceUrl" },
                values: new object[,]
                {
                    { new Guid("5855b7eb-22cc-4ee0-8098-cfe40edb08e0"), "Author4", null, null, null, 400, null, null, null, null, "Test4", null },
                    { new Guid("788d2b4e-e7de-4a6f-b460-dad2791f5fbe"), "Author3", null, null, null, 300, null, null, null, null, "Test3", null },
                    { new Guid("d3597dcb-a3cf-47ba-b797-aff2ae7a4b31"), "Author1", null, null, null, 100, null, null, null, null, "Test1", null },
                    { new Guid("e32496b0-e7ef-4b92-b318-602fd9921969"), "Author5", null, null, null, 500, null, null, null, null, "Test5", null },
                    { new Guid("eb02f5e0-7e59-4966-91c9-ca36f867584f"), "Author2", null, null, null, 200, null, null, null, null, "Test2", null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_AspNetUsers_ApplicationUserId",
                table: "Reacts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reacts_Posts_PostId",
                table: "Reacts",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_AspNetUsers_ApplicationUserId",
                table: "Reacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Reacts_Posts_PostId",
                table: "Reacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reacts",
                table: "Reacts");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("5855b7eb-22cc-4ee0-8098-cfe40edb08e0"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("788d2b4e-e7de-4a6f-b460-dad2791f5fbe"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("d3597dcb-a3cf-47ba-b797-aff2ae7a4b31"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e32496b0-e7ef-4b92-b318-602fd9921969"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("eb02f5e0-7e59-4966-91c9-ca36f867584f"));

            migrationBuilder.RenameTable(
                name: "Reacts",
                newName: "Reactes");

            migrationBuilder.RenameIndex(
                name: "IX_Reacts_PostId",
                table: "Reactes",
                newName: "IX_Reactes_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Reacts_ApplicationUserId",
                table: "Reactes",
                newName: "IX_Reactes_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reactes",
                table: "Reactes",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AverageRating", "Description", "ImageUrl", "NumberOfPages", "PdfUrl", "PublishedYear", "RatingsCount", "ReadingCount", "Title", "VoiceUrl" },
                values: new object[,]
                {
                    { new Guid("28b4208c-f594-430e-af57-41b109199b35"), "Author3", null, null, null, 300, null, null, null, null, "Test3", null },
                    { new Guid("580214fb-113c-4c1b-884d-ca0911aaddbd"), "Author2", null, null, null, 200, null, null, null, null, "Test2", null },
                    { new Guid("9b7a7e38-e810-40f1-9b2d-4d795f46cee5"), "Author4", null, null, null, 400, null, null, null, null, "Test4", null },
                    { new Guid("ceae109a-7be8-4126-8b22-4ae210becb63"), "Author1", null, null, null, 100, null, null, null, null, "Test1", null },
                    { new Guid("e64b4582-0421-4b20-b811-421c4d9326b6"), "Author5", null, null, null, 500, null, null, null, null, "Test5", null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Reactes_AspNetUsers_ApplicationUserId",
                table: "Reactes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reactes_Posts_PostId",
                table: "Reactes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
