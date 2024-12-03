using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shared.Migrations
{
    /// <inheritdoc />
    public partial class testingwithsettingupforeignkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserFileAccesses_FileId",
                table: "UserFileAccesses",
                column: "FileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFileAccesses_UserId",
                table: "UserFileAccesses",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFileAccesses_Files_FileId",
                table: "UserFileAccesses",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFileAccesses_Users_UserId",
                table: "UserFileAccesses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRsaKeyPairs_Users_Id",
                table: "UserRsaKeyPairs",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFileAccesses_Files_FileId",
                table: "UserFileAccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFileAccesses_Users_UserId",
                table: "UserFileAccesses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRsaKeyPairs_Users_Id",
                table: "UserRsaKeyPairs");

            migrationBuilder.DropIndex(
                name: "IX_UserFileAccesses_FileId",
                table: "UserFileAccesses");

            migrationBuilder.DropIndex(
                name: "IX_UserFileAccesses_UserId",
                table: "UserFileAccesses");
        }
    }
}
