using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ahmad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Categories",
                table: "Books",
                newName: "Category");

            migrationBuilder.AddColumn<int>(
                name: "ReadingCount",
                table: "Books",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadingCount",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Books",
                newName: "Categories");
        }
    }
}
