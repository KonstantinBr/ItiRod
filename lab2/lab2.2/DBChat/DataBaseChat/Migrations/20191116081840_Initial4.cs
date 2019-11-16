using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseChat.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatTableId",
                table: "ChatTables",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ChatTables",
                newName: "ChatTableId");
        }
    }
}
