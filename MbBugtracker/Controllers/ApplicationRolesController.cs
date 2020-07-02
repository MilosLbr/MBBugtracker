using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModels;
using MbBugtracker.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using DataModels.ViewModels;
using MbBugtracker.Services.Interfaces;

namespace MbBugtracker.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ApplicationRolesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public ApplicationRolesController( IUnitOfWork unitOfWork ,RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        // GET: ApplicationRoles
        public async Task<IActionResult> Index()
        {
            var allRoles = await _roleManager.Roles.ToListAsync();
            var rolesVm = _mapper.Map<IEnumerable<ApplicationRoleViewModel>>(allRoles);

            return View(rolesVm);
        }

        // GET: ApplicationRoles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _roleManager.FindByIdAsync(id);

            if (applicationRole == null)
            {
                return NotFound();
            }

            return View(applicationRole);
        }

        // GET: ApplicationRoles/Create
        public IActionResult Create()
        {
            var appRoleVM = new ApplicationRoleViewModel();

            return View(appRoleVM);
        }

        // POST: ApplicationRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationRoleViewModel applicationRoleVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new ApplicationRole() { 
                    Name = applicationRoleVM.Name,
                    RoleDescription = applicationRoleVM.RoleDescription
                });

                if (result.Succeeded)
                    return RedirectToAction("Index");
            }
            return View(applicationRoleVM);
        }

        // GET: ApplicationRoles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _roleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            var appRoleVm = _mapper.Map<ApplicationRoleViewModel>(applicationRole);
            return View(appRoleVm);
        }

        // POST: ApplicationRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationRoleViewModel appRoleViewModel)
        {
            if (id != appRoleViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var roleToUpdate = await _roleManager.FindByIdAsync(id);
                _mapper.Map(appRoleViewModel, roleToUpdate);

                if(await _unitOfWork.Complete() > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(appRoleViewModel);
        }

        // GET: ApplicationRoles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicationRole = await _roleManager.FindByIdAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return View(applicationRole);
        }

        // POST: ApplicationRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var applicationRole = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(applicationRole);

            return RedirectToAction(nameof(Index));
        }

    }
}
