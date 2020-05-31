using Microsoft.EntityFrameworkCore.Migrations;

namespace MbBugtracker.Data.Migrations
{
    public partial class RemoveProjectNameColumnFromTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Tickets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
