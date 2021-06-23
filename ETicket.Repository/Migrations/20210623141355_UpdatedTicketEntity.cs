using Microsoft.EntityFrameworkCore.Migrations;

namespace ETicket.Repository.Migrations
{
    public partial class UpdatedTicketEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MovieCategory",
                table: "Tickets",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieCategory",
                table: "Tickets");
        }
    }
}
