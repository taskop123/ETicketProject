using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {
        [Required]
        public string MovieTitle { get; set; }
        [Required]
        public string MovieImage { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public virtual ICollection<TicketsInShoppingCart> ShoppingCarts{ get; set; }
        public virtual ICollection<TicketsInOrder> Orders{ get; set; }

    }
}
