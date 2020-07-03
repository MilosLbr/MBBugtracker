using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MbBugtracker.Data.Migrations
{
    public partial class AddActivityLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityDate = table.Column<DateTime>(nullable: false),
                    ActivityDescription = table.Column<string>(nullable: false),
                    TicketId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketActivityLogs_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TicketActivityLogs_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketActivityLogs_ApplicationUserId",
                table: "TicketActivityLogs",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketActivityLogs_TicketId",
                table: "TicketActivityLogs",
                column: "TicketId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketActivityLogs");
        }
    }
}
