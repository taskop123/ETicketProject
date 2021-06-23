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

namespace ETicket.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService ticketService;

        public TicketsController(ITicketService ticketService)
        {
            this.ticketService = ticketService;
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
    }
}
