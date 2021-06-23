using ETicket.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<TicketsInShoppingCart> TicketsInShoppingCarts{ get; set; }
        public double TotalPrice { get; set; }
    }
}
