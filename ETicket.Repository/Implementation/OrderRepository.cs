using ETicket.Domain.DomainModels;
using ETicket.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<Order>();
        }

        public void Delete(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this.entities.Remove(entity);
            this.context.SaveChanges();
        }

        public IEnumerable<Order> GetUserOrders(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("entity");
            }

            return this.entities
                .Include(z => z.User)
                .Include(z => z.TicketsInOrder)
                .Include("TicketsInOrder.Ticket")
                .Where(z => z.UserId == id); 
        }

        public IEnumerable<Order> GetAll()
        {
            return this.entities
                .Include(z => z.User)
                .Include(z => z.TicketsInOrder)
                .Include("TicketsInOrder.Order")
                .ToList();
        }

        public void Insert(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entities.Add(entity);
            context.SaveChanges();

        }

        public void Update(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            entities.Update(entity);
            context.SaveChanges();
        }

        public Order GetDetails(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("entity");
            }

            return this.entities
                .Include(z => z.User)
                .Include(z => z.TicketsInOrder)
                .Include("TicketsInOrder.Ticket")
                .SingleOrDefault(z => z.Id == id);
        }
    }
}
