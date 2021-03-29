using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_Management_System_Data.Migrations
{
    public partial class freshMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowed_Books_selectedBooks_SelectedBookid",
                table: "Borrowed_Books");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBooks_AspNetUsers_UserId",
                table: "UserBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_selectedBooks",
                table: "selectedBooks");

            migrationBuilder.DropColumn(
                name: "ApplicatioUserId",
                table: "UserBooks");

            migrationBuilder.RenameTable(
                name: "selectedBooks",
                newName: "SelectedBooks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserBooks",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBooks_UserId",
                table: "UserBooks",
                newName: "IX_UserBooks_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SelectedBooks",
                table: "SelectedBooks",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowed_Books_SelectedBooks_SelectedBookid",
                table: "Borrowed_Books",
                column: "SelectedBookid",
                principalTable: "SelectedBooks",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBooks_AspNetUsers_ApplicationUserId",
                table: "UserBooks",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowed_Books_SelectedBooks_SelectedBookid",
                table: "Borrowed_Books");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBooks_AspNetUsers_ApplicationUserId",
                table: "UserBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SelectedBooks",
                table: "SelectedBooks");

            migrationBuilder.RenameTable(
                name: "SelectedBooks",
                newName: "selectedBooks");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "UserBooks",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBooks_ApplicationUserId",
                table: "UserBooks",
                newName: "IX_UserBooks_UserId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicatioUserId",
                table: "UserBooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_selectedBooks",
                table: "selectedBooks",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowed_Books_selectedBooks_SelectedBookid",
                table: "Borrowed_Books",
                column: "SelectedBookid",
                principalTable: "selectedBooks",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBooks_AspNetUsers_UserId",
                table: "UserBooks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
