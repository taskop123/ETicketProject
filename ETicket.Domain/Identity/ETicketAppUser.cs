using ETicket.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.Identity
{
    public class ETicketAppUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Order> Orders{ get; set; }
        public virtual ShoppingCart ShoppingCart{ get; set; }
    }
}
