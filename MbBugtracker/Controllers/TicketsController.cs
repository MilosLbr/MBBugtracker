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
            ticketViewModel.TicketActivityLogs = ticketViewModel.TicketActivityLogs.OrderByDescending(tl => tl.ActivityDate);

            return View(ticketViewModel);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        public IActionResult Create()
        {            
            var viewModel = new TicketCreateEditViewModel();
            PrepareCreateEditViewModel(ref viewModel);

            return View(viewModel);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
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
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
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
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
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

                LogEditActivity(ticketViewModel, ticketToUpdate);

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
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetById(id);
            var userId = _userManager.GetUserId(User);

            if (ticket == null)
            {
                return NotFound();
            }
            if(ticket.ApplicationUserId != userId)
            {
                return RedirectToAction(nameof(Details), new { Id = ticket.Id });
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _unitOfWork.Tickets.GetById(id);

            // clear log
            _unitOfWork.TicketActivityLogs.RemoveRange(ticket.TicketActivityLogs);

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

        private void LogEditActivity(TicketCreateEditViewModel newValues, Ticket oldValues)
        {
            string editedProperties = GetEditedProperties(newValues, oldValues);

            if(editedProperties != null)
            {
                var activityLog = new TicketActivityLog()
                {
                    ActivityDate = DateTime.Now,
                    ActivityDescription = editedProperties,
                    TicketId = oldValues.Id,
                    ApplicationUserId = _userManager.GetUserId(User)
                };

                _unitOfWork.TicketActivityLogs.Add(activityLog);
            }            

        }

        private string GetEditedProperties(TicketCreateEditViewModel newValues, Ticket oldValues)
        {
            var properties = new List<string>
            {
                "Title", "Description", "DueDate",
                "AssignedTo", "ProjectId", "TicketPriorityId", "TicketStatusId",
                "TicketTypeId"
            };

            var editedProperties = new List<string>();

            foreach (var prop in properties)
            {
                var oldValue = oldValues.GetType().GetProperty(prop).GetValue(oldValues, null);
                var newValue = newValues.GetType().GetProperty(prop).GetValue(newValues, null);

                if(oldValue.ToString() != newValue.ToString())
                {
                    editedProperties.Add(prop);
                }
            }            

            if(editedProperties.Count > 0)
            {
                var result = string.Join(", ", editedProperties);

                return "Edited " + result;
            }
            return null;
        }
    }
}
