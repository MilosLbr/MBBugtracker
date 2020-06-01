using Microsoft.EntityFrameworkCore.Migrations;

namespace MbBugtracker.Data.Migrations
{
    public partial class AddTicketTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketTypeId",
                table: "Tickets",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TicketTypes",
                columns: new[] { "Id", "TypeName" },
                values: new object[,]
                {
                    { 1, "Code bug" },
                    { 2, "Crash/Hang" },
                    { 3, "Data loss" },
                    { 4, "Performance" },
                    { 5, "UI" },
                    { 6, "New Feature" },
                    { 7, "Enhancement" },
                    { 8, "Security" },
                    { 9, "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketTypeId",
                table: "Tickets",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketTypes_TicketTypeId",
                table: "Tickets",
                column: "TicketTypeId",
                principalTable: "TicketTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketTypes_TicketTypeId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TicketTypeId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketTypeId",
                table: "Tickets");
        }
    }
}
