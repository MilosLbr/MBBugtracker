using Microsoft.EntityFrameworkCore.Migrations;

namespace MbBugtracker.Data.Migrations
{
    public partial class AddProjectStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStatuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ProjectStatuses",
                columns: new[] { "Id", "StatusName" },
                values: new object[,]
                {
                    {1, "Active" },
                    {2, "In Progress" },
                    {3, "On Track" },
                    {4, "Delayed" },
                    {5, "In Testing" },
                    {6, "On Hold" },
                    {7, "Approved" },
                    {8, "Cancelled" },
                    {9, "Planning" },
                    {10, "Completed" },
                    {11, "Invoiced" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectStatuses");
        }
    }
}
