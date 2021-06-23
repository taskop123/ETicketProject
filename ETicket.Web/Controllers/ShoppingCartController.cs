using ETicket.Domain.DTO;
using ETicket.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ETicket.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingCartDto shoppingCartDto = this.shoppingCartService.GetShoppingCartInfo(userId);

            return View(shoppingCartDto);
        }

        public IActionResult DeleteTicketFromShoppingCart(Guid ticketId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.shoppingCartService.DeleteProductFromShoppingCart(userId, ticketId);

            return RedirectToAction("Index");
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this.shoppingCartService.GetShoppingCartInfo(userId);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var result = chargeService.Create(new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(order.TotalPrice) * 100,
                Description = "E Ticket App Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (result.Status == "succeeded")
            {
                var res = this.shoppingCartService.OrderNow(userId);
                if (res)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");
        }

    }
}
