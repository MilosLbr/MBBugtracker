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

namespace MbBugtracker.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            var projectDetailsViewModel = _mapper.Map<ProjectDetailsViewModel>(project);
            return View(projectDetailsViewModel);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var projectEditViewModel = _mapper.Map<ProjectEditViewModel>(project);
            projectEditViewModel.SelectedUserIds = new List<string>();

            foreach (var projUser in projectEditViewModel.ProjectsAndUsers)
            {
                projectEditViewModel.SelectedUserIds.Add(projUser.ApplicationUserId);
            }
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
                try
                {
                    var projectToEdit = await _context.Projects.FindAsync(projectEditViewModel.Id);

                    _mapper.Map(projectEditViewModel, projectToEdit);

                    // update ProjectsAndUsers table
                    var projectUsers = new List<ProjectsAndUsers>();

                    foreach (var userId in projectEditViewModel.SelectedUserIds)
                    {
                        var projectUser = new ProjectsAndUsers
                        {
                            ApplicationUserId = userId,
                        };

                        projectUsers.Add(projectUser);
                    }

                    projectToEdit.ProjectsAndUsers = projectUsers;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(projectEditViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }  
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var project = await _context.Projects.FindAsync(id);
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
