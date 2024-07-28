using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookMate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class favv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("19d5194b-296e-431e-83a8-d177268c3aca"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("20cd6a45-774e-45f8-a1a8-7a3564230e3d"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("2d58fcf1-2642-4281-a2be-38a3b9e6e95d"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("538f435b-0d0e-4411-a4ef-b11a815d532f"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("b7fa1705-2fce-4a7c-8fa5-0b1c96ab3df0"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { new Guid("19d5194b-296e-431e-83a8-d177268c3aca"), "Author5", null, null, null, 500, null, null, null, null, "Test5", null },
                    { new Guid("20cd6a45-774e-45f8-a1a8-7a3564230e3d"), "Author2", null, null, null, 200, null, null, null, null, "Test2", null },
                    { new Guid("2d58fcf1-2642-4281-a2be-38a3b9e6e95d"), "Author1", null, null, null, 100, null, null, null, null, "Test1", null },
                    { new Guid("538f435b-0d0e-4411-a4ef-b11a815d532f"), "Author4", null, null, null, 400, null, null, null, null, "Test4", null },
                    { new Guid("b7fa1705-2fce-4a7c-8fa5-0b1c96ab3df0"), "Author3", null, null, null, 300, null, null, null, null, "Test3", null }
                });
        }
    }
}
