using ETicket.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETicket.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto GetShoppingCartInfo(string userId);
        bool DeleteProductFromShoppingCart(string userId, Guid id);
        bool OrderNow(string userId);
    }
}
