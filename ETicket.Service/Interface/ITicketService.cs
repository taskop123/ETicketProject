using ETicket.Domain.DomainModels;
using ETicket.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Service.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Ticket GetDetailsForTicket(Guid? id);
        void CreateNewTicket(Ticket t);
        void UpdateExistingTicket(Ticket t);
        AddTicketToCartDto GetShoppingCartInfo(Guid? id);
        void DeleteTicket(Guid? id);
        bool AddToShoppingCart(AddTicketToCartDto item, string userId);
        List<Ticket> GetTicketsByCategory(string category);
    }
}
