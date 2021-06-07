using ETicket.Domain.Identity;
using ETicket.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETicket.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext context;
        private DbSet<ETicketAppUser> entities;
        string errormessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<ETicketAppUser>();
        }

        public void Delete(ETicketAppUser entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

        public ETicketAppUser Get(string id)
        {
            return entities
                .Include(z => z.ShoppingCart)
                .Include("ShoppingCart.TicketsInShoppingCart")
                .Include("ShoppingCart.TicketsInShoppingCart.Ticket")
                .SingleOrDefault(z => z.Id == id);
        }

        public IEnumerable<ETicketAppUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public void Insert(ETicketAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(ETicketAppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }
    }
}
