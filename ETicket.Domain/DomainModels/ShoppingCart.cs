using ETicket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual ETicketAppUser Owner{ get; set; }
        public virtual ICollection<TicketsInShoppingCart> TicketsInShoppingCart{ get; set; }
    }
}
