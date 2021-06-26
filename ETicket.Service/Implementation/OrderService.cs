using ETicket.Domain.DomainModels;
using ETicket.Repository.Interface;
using ETicket.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public IEnumerable<Order> GetAllOrders()
        {
            return this.orderRepository.GetAll();
        }

        public IEnumerable<Order> GetAllOrdersForUser(string id)
        {
            return this.orderRepository.GetUserOrders(id);
        }

        public Order GetOrderDetails(Guid? id)
        {
            return this.orderRepository.GetDetails(id);
        }
    }
}
