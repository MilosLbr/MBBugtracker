using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModels;
using MbBugtracker.Data;
using AutoMapper;
using DataModels.ViewModels;
using MbBugtracker.Services.Interfaces;
using DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MbBugtracker.Controllers
{

    public class ProjectsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var myProjects = await _unitOfWork.Projects.GetProjectsForCurrentUser(currentUser);

            var myProjectsViewModel = _mapper.Map<IEnumerable<ProjectDetailsViewModel>>(myProjects);

            return View(myProjectsViewModel);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var project = await _unitOfWork.Projects
                .GetById(id);

            if (project == null)
            {
                return NotFound();
            }
            var currentUserId = _userManager.GetUserId(User);
            ViewBag.userId = currentUserId;

            var projectDetailsViewModel = _mapper.Map<ProjectDetailsViewModel>(project);

            //order 
            projectDetailsViewModel.ProjectTickets = projectDetailsViewModel.ProjectTickets
                .OrderBy(t => {
                    if (t.TicketStatus.Id == 2)
                    {
                        return 5;
                    }
                    else if (t.TicketStatus.Id == 5)
                    {
                        return 4;
                    }
                    else
                    {
                        return t.TicketStatus.Id;
                    }
                })
                .ThenByDescending(t => t.TicketPriority.Id)
                .ToList();

            return View(projectDetailsViewModel);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> Create()
        {
            ViewBag.projectStatuses = await _unitOfWork.ProjectStatuses.GetAll();
            return View();
        }

        // POST: Projects/Create
        [Authorize(Roles = "Admin,ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Projects.Add(project);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _unitOfWork.Projects.GetById(id);

            if (project == null)
            {
                return NotFound();
            }
            var projectStatuses = await _unitOfWork.ProjectStatuses.GetAll();
            var projectStatusesDto = _mapper.Map<IEnumerable<ProjectStatusDto>>(projectStatuses);
            
            var projectEditViewModel = _mapper.Map<ProjectEditViewModel>(project);
            projectEditViewModel.ProjectStatuses = projectStatusesDto;

            return View(projectEditViewModel);
        }

        // POST: Projects/Edit/5
        [Authorize(Roles = "Admin,ProjectManager")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody]ProjectEditViewModel projectEditViewModel)
        {
            if (id != projectEditViewModel.Id)
            {
                return NotFound();
            }
            if(string.IsNullOrWhiteSpace(projectEditViewModel.ProjectName) || string.IsNullOrWhiteSpace(projectEditViewModel.Description) || projectEditViewModel.SelectedUserIds.Count == 0)
            {
                return BadRequest("All fields are required");
            }
            else
            {
                var projectToEdit = await _unitOfWork.Projects.GetById(projectEditViewModel.Id);

                _mapper.Map(projectEditViewModel, projectToEdit);

                // update ProjectsAndUsers table
                var projectUsers = new List<ProjectsAndUsers>();

                foreach (var userId in projectEditViewModel.SelectedUserIds)
                {
                    var projectUser = new ProjectsAndUsers
                    {
                        ApplicationUserId = userId,
                        ProjectId = id
                    };

                    projectUsers.Add(projectUser);
                }

                projectToEdit.ProjectsAndUsers = projectUsers;

                await _unitOfWork.Complete();
                
                return RedirectToAction(nameof(Index));
            }  
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _unitOfWork.Projects.GetById(id);

            if (project == null)
            {
                return NotFound();
            }
            var currentUserId = _userManager.GetUserId(User);
            if(currentUserId != project.ApplicationUserId)
            {
                return RedirectToAction(nameof(Details), new { Id = project.Id });
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin,ProjectManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _unitOfWork.Projects.GetById(id);
            _unitOfWork.Projects.Remove(project);
            await _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

    }
}
