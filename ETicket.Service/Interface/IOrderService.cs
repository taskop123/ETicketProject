using ETicket.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Service.Interface
{
    public interface IOrderService
    {

        IEnumerable<Order> GetAllOrdersForUser(string id);
        IEnumerable<Order> GetAllOrders();
        Order GetOrderDetails(Guid? id);

    }
}
