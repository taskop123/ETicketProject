using ETicket.Domain.DomainModels;
using ETicket.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ETicketAppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<TicketsInOrder> TicketsInOrder { get; set; }
        public virtual DbSet<TicketsInShoppingCart> TicketsInShoppingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ticket>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

           /* builder.Entity<TicketsInShoppingCart>()
                .HasKey(z => new { z.TicketId, z.ShoppingCartId });*/

            builder.Entity<TicketsInShoppingCart>()
                .HasOne(z => z.Ticket)
                .WithMany(z => z.ShoppingCarts)
                .HasForeignKey(z => z.TicketId);

            builder.Entity<TicketsInShoppingCart>()
                .HasOne(z => z.ShoppingCart)
                .WithMany(z => z.TicketsInShoppingCart)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<ShoppingCart>()
                .HasOne<ETicketAppUser>(z => z.Owner)
                .WithOne(z => z.ShoppingCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);

           /* builder.Entity<TicketsInOrder>()
                .HasKey(z => new { z.TicketId, z.OrderId });*/

            builder.Entity<TicketsInOrder>()
                .HasOne(z => z.Ticket)
                .WithMany(z => z.Orders)
                .HasForeignKey(z => z.TicketId);

            builder.Entity<TicketsInOrder>()
                .HasOne(z => z.Order)
                .WithMany(z => z.TicketsInOrder)
                .HasForeignKey(z => z.OrderId);
        }
    }
}
