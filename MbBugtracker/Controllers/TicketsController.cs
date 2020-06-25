using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataModels;
using DataModels.ViewModels;
using MbBugtracker.Data;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace MbBugtracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public TicketsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.userId = userId;

            var tickets = await _context.Tickets.ToListAsync();

            var ticketList = _mapper.Map<List<TicketListViewModel>>(tickets);

            foreach (var ticket in ticketList)
            {
                var updatedByUserId = ticket.UpdatedByUserId;
                if(updatedByUserId != null)
                {
                    var user = await _userManager.FindByIdAsync(updatedByUserId);
                    var userName = user.UserName;

                    ticket.UpdatedByUserName = userName;
                }
            }

            return View(ticketList);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.userId = userId;

            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            var ticketViewModel = _mapper.Map<TicketDetailsViewModel>(ticket);

            ticketViewModel.TicketComments = ticketViewModel.TicketComments.OrderByDescending(tc => tc.DateAdded);
            return View(ticketViewModel);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            
            var viewModel = new TicketCreateEditViewModel();
            PrepareCreateEditViewModel(ref viewModel);

            return View(viewModel);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketCreateEditViewModel ticket)
        {            

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                ticket.ApplicationUserId = userId;
                ticket.CreatedOn = DateTime.Now;
                ticket.UpdatedOn = DateTime.Now;

                var ticketToAdd = _mapper.Map<TicketCreateEditViewModel, Ticket>(ticket);

                _context.Add(ticketToAdd);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                PrepareCreateEditViewModel(ref ticket);
                return View(ticket);
            }
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var ticketViewModel = _mapper.Map<TicketCreateEditViewModel>(ticket);
            PrepareCreateEditViewModel(ref ticketViewModel);

            return View(ticketViewModel);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TicketCreateEditViewModel ticketViewModel)
        {
            var userId = _userManager.GetUserId(User);

            if (id != ticketViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var ticketToUpdate = _context.Tickets.Find(ticketViewModel.Id);
                
                _mapper.Map(ticketViewModel, ticketToUpdate);
                ticketToUpdate.UpdatedOn = DateTime.Now;
                ticketToUpdate.UpdatedByUserId = userId;

                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            PrepareCreateEditViewModel(ref ticketViewModel);

            return View(ticketViewModel);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

        private void PrepareCreateEditViewModel(ref TicketCreateEditViewModel ticketCreateEditViewModel)
        {
            ticketCreateEditViewModel.AllAppUsers = _context.Users.ToList();
            ticketCreateEditViewModel.AllProjects = _context.Projects.ToList();
            ticketCreateEditViewModel.AllTicketPriorities = _context.TicketPriorities.ToList();
            ticketCreateEditViewModel.AllTicketStatuses = _context.TicketStatuses.ToList();
            ticketCreateEditViewModel.AllTicketTypes = _context.TicketTypes.ToList();
        }
    }
}
