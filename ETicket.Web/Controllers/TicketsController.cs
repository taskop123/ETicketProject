using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ETicket.Service.Interface;
using ETicket.Domain.DTO;
using ETicket.Domain.DomainModels;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using System.IO;

namespace ETicket.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService ticketService;

        public TicketsController(ITicketService ticketService)
        {
            this.ticketService = ticketService;
        }

        //GET: Tickets in ascending order by date
        public IActionResult SortTicketsByDateAsc()
        {
            var tickets = this.ticketService.GetAllTickets();
            tickets.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            return View("Index", tickets);
        }
        //GET: Tickets in descending order by date
        public IActionResult SortTicketsByDateDesc()
        {
            var tickets = this.ticketService.GetAllTickets();
            tickets.Sort((x, y) => DateTime.Compare(y.Date, x.Date));
            return View("Index", tickets);
        }
        // GET: Tickets
        public IActionResult Index()
        {
            var allTickets = this.ticketService.GetAllTickets();
            return View(allTickets);
        }
        [HttpGet]
        public IActionResult AddToShoppingCart(Guid? id)
        {

            AddTicketToCartDto model = this.ticketService.GetShoppingCartInfo(id);

            return View(model);
        }
        [HttpPost]
        public IActionResult AddToShoppingCart([Bind("TicketId", "Quantity")] AddTicketToCartDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this.ticketService.AddToShoppingCart(model, userId);

            if (result)
            {
                return RedirectToAction("Index", "Tickets");
            }

            return View(model);

        }

        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this.ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,MovieTitle,MovieCategory,MovieImage,Price,Description,Date")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                this.ticketService.CreateNewTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this.ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,MovieTitle,MovieCategory,MovieImage,Price,Description,Date")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                this.ticketService.UpdateExistingTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this.ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this.ticketService.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return this.ticketService.GetDetailsForTicket(id) != null;
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult TicketExport()
        {
            TicketExportDto model = new TicketExportDto();
            model.Categories.Add("Comedy");
            model.Categories.Add("Thriller");
            model.Categories.Add("Action");
            model.Categories.Add("Family");
            model.Categories.Add("Fantasy");
            model.Categories.Add("Cartoon");

            return View(model);
        }
        [HttpPost, Authorize(Roles = "Admin")]
        public FileContentResult TicketExport(TicketExportDto model)
        {
            List<Ticket> ticketsByCategory = this.ticketService.GetTicketsByCategory(model.ChosenCategory);

            string fileName = model.ChosenCategory + "Tickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using(var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All tickets");

                worksheet.Cell(1, 1).Value = "Category";
                worksheet.Cell(1, 2).Value = model.ChosenCategory;
                worksheet.Cell(2, 1).Value = "Ticket No.";
                worksheet.Cell(2, 2).Value = "Movie Title";
                worksheet.Cell(2, 3).Value = "Movie Price";
                worksheet.Cell(2, 4).Value = "Movie Description";
                worksheet.Cell(2, 5).Value = "Date";

                for (int i = 0; i < ticketsByCategory.Count; i++)
                {
                    var ticket = ticketsByCategory[i];

                    worksheet.Cell(3 + i, 1).Value = (i + 1);
                    worksheet.Cell(3 + i, 2).Value = ticket.MovieTitle;
                    worksheet.Cell(3 + i, 3).Value = "$" + ticket.Price;
                    worksheet.Cell(3 + i, 4).Value = ticket.Description;
                    worksheet.Cell(3 + i, 5).Value = ticket.Date.ToLongDateString().ToString();
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    return File(stream.ToArray(), contentType, fileName);
                }
            }
        }
    }
}
