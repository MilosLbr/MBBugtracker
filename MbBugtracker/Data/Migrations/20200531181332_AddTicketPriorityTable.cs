using Microsoft.EntityFrameworkCore.Migrations;

namespace MbBugtracker.Data.Migrations
{
    public partial class AddTicketPriorityTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "TicketPriorityId",
                table: "Tickets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TicketPriorities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriorityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPriorities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketPriorityId",
                table: "Tickets",
                column: "TicketPriorityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketPriorities_TicketPriorityId",
                table: "Tickets",
                column: "TicketPriorityId",
                principalTable: "TicketPriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketPriorities_TicketPriorityId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "TicketPriorities");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TicketPriorityId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketPriorityId",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
