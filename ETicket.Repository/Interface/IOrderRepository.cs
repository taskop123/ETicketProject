using ETicket.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Repository.Interface
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetUserOrders(string id);
        Order GetDetails(Guid? id);
        void Insert(Order entity);
        void Update(Order entity);
        void Delete(Order entity);
    }
}
