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

namespace MbBugtracker.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Projects.GetAll());
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

            var projectDetailsViewModel = _mapper.Map<ProjectDetailsViewModel>(project);
            return View(projectDetailsViewModel);
        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.projectStatuses = await _unitOfWork.ProjectStatuses.GetAll();
            return View();
        }

        // POST: Projects/Create
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
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _unitOfWork.Projects.GetById(id);

            if (project == null)
            {
                return NotFound();
            }

            var projectEditViewModel = _mapper.Map<ProjectEditViewModel>(project);

            return View(projectEditViewModel);
        }

        // POST: Projects/Edit/5
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
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _unitOfWork.Projects.GetById(id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
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
