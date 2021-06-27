using ETicket.Domain.DomainModels;
using ETicket.Domain.DTO;
using ETicket.Domain.Identity;
using ETicket.Repository.Interface;
using ETicket.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> shoppingCartRepository;
        private readonly IUserRepository userRepository;
        private readonly IRepository<EmailMessage> emailRepository;
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<TicketsInOrder> ticketsInOrderRepository;
        private readonly IEmailService emailService;

        public ShoppingCartService(IEmailService emailService, IRepository<EmailMessage> emailRepository, IRepository<TicketsInOrder> ticketsInOrderRepository, IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<TicketsInShoppingCart> ticketsInShoppingCartsRepository, IRepository<Order> orderRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.userRepository = userRepository;;
            this.orderRepository = orderRepository;
            this.ticketsInOrderRepository = ticketsInOrderRepository;
            this.emailRepository = emailRepository;
            this.emailService = emailService;
        }

        public bool DeleteProductFromShoppingCart(string userId, Guid id)
        {
            if (string.IsNullOrEmpty(userId) || id == null)
            {
                return false;
            }

            ETicketAppUser user = this.userRepository.Get(userId);
            ShoppingCart shoppingCart = user.ShoppingCart;

            var result = shoppingCart.TicketsInShoppingCart.Remove(shoppingCart.TicketsInShoppingCart.Where(z => z.TicketId == id).FirstOrDefault());

            if (result)
            {
                this.shoppingCartRepository.Update(shoppingCart);
                
                return true;
            }
            else
            {
                return false;
            }

        }

        public ShoppingCartDto GetShoppingCartInfo(string userId)
        {
            ShoppingCartDto model = new ShoppingCartDto();

            ETicketAppUser user = this.userRepository.Get(userId);

            ShoppingCart cart = user.ShoppingCart;

            var ticketPrice = cart.TicketsInShoppingCart.Select(z => new
            {
                TicketPrice = z.Ticket.Price,
                Quantity = z.Quantity
            }).ToList();

            float totalPrice = 0;

            foreach(var item in ticketPrice)
            {
                totalPrice += (float)item.Quantity * item.TicketPrice;
            }

            model.TotalPrice = totalPrice;
            model.TicketsInShoppingCarts = cart.TicketsInShoppingCart.ToList();            

            return model;
        }

        public bool OrderNow(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            ETicketAppUser user = this.userRepository.Get(userId);
            ShoppingCart shoppingCart = user.ShoppingCart;

            Order order = new Order();
            order.Id = Guid.NewGuid();
            order.User = user;
            order.UserId = userId;

            this.orderRepository.Insert(order);

            ICollection<TicketsInOrder> tickets = new LinkedList<TicketsInOrder>();
            foreach (var item in shoppingCart.TicketsInShoppingCart)
            {
                TicketsInOrder ticketsInOrder = new TicketsInOrder
                {
                    Id = Guid.NewGuid(),
                    Order = order,
                    OrderId = order.Id,
                    Ticket = item.Ticket,
                    TicketId = item.TicketId,
                    Quantity = item.Quantity
                };
                tickets.Add(ticketsInOrder);
                this.ticketsInOrderRepository.Insert(ticketsInOrder);
            }

            order.TicketsInOrder = tickets;
            this.orderRepository.Update(order);

            user.ShoppingCart.TicketsInShoppingCart.Clear();
            this.userRepository.Update(user);

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.MailTo = user.Email;
            emailMessage.Subject = "Successfully Created Order!";
            emailMessage.Status = false;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Your order is completed. The Order contains: ");
            var totalPrice = 0.0;

            for (int i = 0; i < tickets.ToList().Count; i++)
            {
                var ticket = tickets.ToList()[i];
                sb.AppendLine((i + 1).ToString() + ". Movie Title: " + ticket.Ticket.MovieTitle + " with price of: $" + ticket.Ticket.Price + " and quantity: " + ticket.Quantity);
                totalPrice += ticket.Quantity * ticket.Ticket.Price;
            }

            sb.AppendLine("Total : $" + totalPrice.ToString());
            emailMessage.Content = sb.ToString();

            this.emailRepository.Insert(emailMessage);
            this.emailService.SendEmailAsync(emailMessage);


            return true;
        }
    }
}
