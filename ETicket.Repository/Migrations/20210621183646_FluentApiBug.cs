using Microsoft.EntityFrameworkCore.Migrations;

namespace ETicket.Repository.Migrations
{
    public partial class FluentApiBug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInShoppingCarts_Tickets_ShoppingCartId",
                table: "TicketsInShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInShoppingCarts_ShoppingCarts_TicketId",
                table: "TicketsInShoppingCarts");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInShoppingCarts_ShoppingCarts_ShoppingCartId",
                table: "TicketsInShoppingCarts",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInShoppingCarts_Tickets_TicketId",
                table: "TicketsInShoppingCarts",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInShoppingCarts_ShoppingCarts_ShoppingCartId",
                table: "TicketsInShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketsInShoppingCarts_Tickets_TicketId",
                table: "TicketsInShoppingCarts");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInShoppingCarts_Tickets_ShoppingCartId",
                table: "TicketsInShoppingCarts",
                column: "ShoppingCartId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsInShoppingCarts_ShoppingCarts_TicketId",
                table: "TicketsInShoppingCarts",
                column: "TicketId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
