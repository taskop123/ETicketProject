using ETicket.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicket.Domain.DTO
{
    public class AddTicketToCartDto
    {
        public Ticket Ticket{ get; set; }
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
