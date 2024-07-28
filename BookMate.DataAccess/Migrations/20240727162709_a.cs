using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookMate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("1ebcf823-2304-4340-bc13-dfdfcce028af"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("4d8b8baf-a66d-459f-81b6-dc41967aaaac"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("4f13d7ae-c156-493b-9f8a-ac9ee6b6cca3"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("bcc11094-5f00-4ffd-aa22-2416d14e0277"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("fcb866d4-9958-4185-908a-72b6c7218902"));

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AverageRating", "Description", "ImageUrl", "NumberOfPages", "PdfUrl", "PublishedYear", "RatingsCount", "ReadingCount", "Title", "VoiceUrl" },
                values: new object[,]
                {
                    { new Guid("0072fa3d-f8d7-4f3f-bf20-9e98822ecc15"), "Author1", null, null, null, 100, null, null, null, null, "Test1", null },
                    { new Guid("82a9cc77-3176-4f48-8565-659d474018b1"), "Author4", null, null, null, 400, null, null, null, null, "Test4", null },
                    { new Guid("b9a6ea16-2da2-470d-8821-27a9a498cbc3"), "Author5", null, null, null, 500, null, null, null, null, "Test5", null },
                    { new Guid("d79fb358-ea28-4c64-9db6-08346120c340"), "Author3", null, null, null, 300, null, null, null, null, "Test3", null },
                    { new Guid("f1a34c65-c0f2-4cdb-a3ac-cdbf77582870"), "Author2", null, null, null, 200, null, null, null, null, "Test2", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("0072fa3d-f8d7-4f3f-bf20-9e98822ecc15"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("82a9cc77-3176-4f48-8565-659d474018b1"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b9a6ea16-2da2-470d-8821-27a9a498cbc3"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("d79fb358-ea28-4c64-9db6-08346120c340"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("f1a34c65-c0f2-4cdb-a3ac-cdbf77582870"));

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AverageRating", "Description", "ImageUrl", "NumberOfPages", "PdfUrl", "PublishedYear", "RatingsCount", "ReadingCount", "Title", "VoiceUrl" },
                values: new object[,]
                {
                    { new Guid("1ebcf823-2304-4340-bc13-dfdfcce028af"), "Author3", null, null, null, 300, null, null, null, null, "Test3", null },
                    { new Guid("4d8b8baf-a66d-459f-81b6-dc41967aaaac"), "Author1", null, null, null, 100, null, null, null, null, "Test1", null },
                    { new Guid("4f13d7ae-c156-493b-9f8a-ac9ee6b6cca3"), "Author5", null, null, null, 500, null, null, null, null, "Test5", null },
                    { new Guid("bcc11094-5f00-4ffd-aa22-2416d14e0277"), "Author4", null, null, null, 400, null, null, null, null, "Test4", null },
                    { new Guid("fcb866d4-9958-4185-908a-72b6c7218902"), "Author2", null, null, null, 200, null, null, null, null, "Test2", null }
                });
        }
    }
}
