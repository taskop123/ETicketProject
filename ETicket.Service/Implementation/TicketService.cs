using ETicket.Domain.DomainModels;
using ETicket.Domain.DTO;
using ETicket.Repository.Interface;
using ETicket.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.Service.Implementation
{
    public class TicketService : ITicketService
    {

        private readonly IRepository<Ticket> ticketRepository;
        private readonly IUserRepository userRepository;
        private readonly IRepository<TicketsInShoppingCart> ticketsInShoppingCartRepository;
        public TicketService(IRepository<Ticket> repository, IUserRepository userRepository, IRepository<TicketsInShoppingCart> ticketsInShoppingCartRepository)
        {
            this.ticketRepository = repository;
            this.userRepository = userRepository;
            this.ticketsInShoppingCartRepository = ticketsInShoppingCartRepository;
        }

        public bool AddToShoppingCart(AddTicketToCartDto item, string userId)
        {
            var user = this.userRepository.Get(userId);
            var cart = user.ShoppingCart;

            if(item.TicketId != null && cart != null)
            {
                Ticket ticket = this.ticketRepository.Get(item.TicketId);
               
                IList<TicketsInShoppingCart> ticketsInShoppingCarts = this.ticketsInShoppingCartRepository.GetAll().ToList();

                foreach (var i in ticketsInShoppingCarts)
                {
                    if (i.TicketId.Equals(item.TicketId) && i.ShoppingCartId.Equals(cart.Id))
                    {
                        var ticketInShoppingCart = this.ticketsInShoppingCartRepository.Get(i.Id);
                        ticketInShoppingCart.Quantity += item.Quantity;

                        this.ticketsInShoppingCartRepository.Update(ticketInShoppingCart);

                        return true;
                    }
                }

                if(ticket != null)
                {
                    TicketsInShoppingCart model = new TicketsInShoppingCart
                    {
                        ShoppingCart = cart,
                        ShoppingCartId = cart.Id,
                        Ticket = ticket,
                        TicketId = ticket.Id,
                        Quantity = item.Quantity
                    };
                    this.ticketsInShoppingCartRepository.Insert(model);
                    return true;
                }
                return false;

            }
            return false;

        }

        public void CreateNewTicket(Ticket t)
        {
            ticketRepository.Insert(t);
        }

        public void DeleteTicket(Guid? id)
        {
            var ticket = ticketRepository.Get(id);
            ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            return ticketRepository.GetAll().ToList();
        }

        public Ticket GetDetailsForTicket(Guid? id)
        {
            return ticketRepository.Get(id);
        }

        public AddTicketToCartDto GetShoppingCartInfo(Guid? id)
        {
            Ticket t = GetDetailsForTicket(id);

            AddTicketToCartDto model = new AddTicketToCartDto
            {
                Ticket = t,
                TicketId = t.Id,
                Quantity = 1
            };
            return model;
        }

        public List<Ticket> GetTicketsByCategory(string category)
        {
            return this.ticketRepository.GetAll()
                .Where(z => z.MovieCategory.Equals(category))
                .ToList();
        }

        public void UpdateExistingTicket(Ticket t)
        {
            ticketRepository.Update(t);
        }
    }
}
