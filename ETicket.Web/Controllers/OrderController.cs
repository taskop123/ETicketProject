using ETicket.Domain.DomainModels;
using ETicket.Domain.Identity;
using ETicket.Service.Interface;
using ExcelDataReader;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ETicket.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly UserManager<ETicketAppUser> userManager;
       
        public OrderController(IOrderService orderService, UserManager<ETicketAppUser> userManager)
        {
            this.orderService = orderService;
            this.userManager = userManager;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Order> specificUserOrders = this.orderService.GetAllOrdersForUser(userId);

            return View(specificUserOrders);
        }
        public FileContentResult CreateInvoice(Guid? id)
        {
            Order order = this.orderService.GetOrderDetails(id);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", order.Id.ToString());
            document.Content.Replace("{{UserName}}", order.User.Email);

            StringBuilder sb = new StringBuilder();
            var totalPrice = 0.0;

            foreach(var ticket in order.TicketsInOrder)
            {
                sb.Append("Movie Title: ").Append(ticket.Ticket.MovieTitle).Append(", date: ").Append(ticket.Ticket.Date).Append(" with price: $")
                    .Append(ticket.Ticket.Price).Append(" and quantity: ").Append(ticket.Quantity).Append("\n");
                totalPrice += ticket.Quantity * ticket.Ticket.Price;
            }

            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", "$" + totalPrice.ToString());

            MemoryStream ms = new MemoryStream();
            document.Save(ms, new PdfSaveOptions());

            return File(ms.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }
        
    }
}
