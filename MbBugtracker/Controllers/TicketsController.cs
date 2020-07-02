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
using MbBugtracker.Services.Interfaces;

namespace MbBugtracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TicketsController(UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.userId = userId;                       

            return View();
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            ViewBag.userId = userId;

            var ticket = await _unitOfWork.Tickets.GetById(id);

            if (ticket == null)
            {
                return NotFound();
            }

            var ticketViewModel = _mapper.Map<TicketDetailsViewModel>(ticket);

            ticketViewModel.TicketStatuses = await _unitOfWork.TicketStatuses.GetAll();

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

                _unitOfWork.Tickets.Add(ticketToAdd);
                await _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                PrepareCreateEditViewModel(ref ticket);
                return View(ticket);
            }
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var ticket = await _unitOfWork.Tickets.GetById(id);
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
                var ticketToUpdate = await _unitOfWork.Tickets.GetById(ticketViewModel.Id);
                
                _mapper.Map(ticketViewModel, ticketToUpdate);
                ticketToUpdate.UpdatedOn = DateTime.Now;
                ticketToUpdate.UpdatedByUserId = userId;

                await _unitOfWork.Complete();
                
                return RedirectToAction(nameof(Index));
            }

            PrepareCreateEditViewModel(ref ticketViewModel);
            return View(ticketViewModel);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetById(id);

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
            var ticket = await _unitOfWork.Tickets.GetById(id);
            _unitOfWork.Tickets.Remove(ticket);

            await _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }


        private void PrepareCreateEditViewModel(ref TicketCreateEditViewModel ticketCreateEditViewModel)
        {
            ticketCreateEditViewModel.AllAppUsers = _userManager.Users.ToList();
            ticketCreateEditViewModel.AllProjects = _unitOfWork.Projects.GetAllSynchronously();
            ticketCreateEditViewModel.AllTicketPriorities = _unitOfWork.TicketPriorities.GetAllSynchronously();
            ticketCreateEditViewModel.AllTicketStatuses = _unitOfWork.TicketStatuses.GetAllSynchronously();
            ticketCreateEditViewModel.AllTicketTypes = _unitOfWork.TicketTypes.GetAllSynchronously();
        }
    }
}
