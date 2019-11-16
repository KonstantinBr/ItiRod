using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseChat.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "ChatTables");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ChatTables",
                newName: "ChatTableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatTables",
                table: "ChatTables",
                column: "ChatTableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatTables",
                table: "ChatTables");

            migrationBuilder.RenameTable(
                name: "ChatTables",
                newName: "Messages");

            migrationBuilder.RenameColumn(
                name: "ChatTableId",
                table: "Messages",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");
        }
    }
}
