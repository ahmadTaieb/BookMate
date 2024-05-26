using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMate.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Librarys_UserId",
                table: "Librarys");

            migrationBuilder.RenameColumn(
                name: "NumberOfPage",
                table: "Books",
                newName: "NumberOfPages");

            migrationBuilder.AlterColumn<string>(
                name: "PublishedYear",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "applicationUserClubs",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClubId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicationUserClubs", x => new { x.ApplicationUserId, x.ClubId });
                    table.ForeignKey(
                        name: "FK_applicationUserClubs_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applicationUserClubs_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookLibraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LibraryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLibraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookLibraries_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookLibraries_Librarys_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Librarys",
                        principalColumn: "LibraryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Librarys_UserId",
                table: "Librarys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_applicationUserClubs_ClubId",
                table: "applicationUserClubs",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_BookLibraries_BookId",
                table: "BookLibraries",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookLibraries_LibraryId",
                table: "BookLibraries",
                column: "LibraryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applicationUserClubs");

            migrationBuilder.DropTable(
                name: "BookLibraries");

            migrationBuilder.DropIndex(
                name: "IX_Librarys_UserId",
                table: "Librarys");

            migrationBuilder.RenameColumn(
                name: "NumberOfPages",
                table: "Books",
                newName: "NumberOfPage");

            migrationBuilder.AlterColumn<int>(
                name: "PublishedYear",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Librarys_UserId",
                table: "Librarys",
                column: "UserId",
                unique: true);
        }
    }
}
