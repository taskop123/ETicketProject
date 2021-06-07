using ETicket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public ETicketAppUser User{ get; set; }
        public virtual ICollection<TicketsInOrder> TicketsInOrder{ get; set; }
    }
}
