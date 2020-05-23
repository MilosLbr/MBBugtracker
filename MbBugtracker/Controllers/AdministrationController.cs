using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using DTOs;
using MbBugtracker.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MbBugtracker.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public AdministrationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Administration
        public ActionResult Index()
        {
            var users = _context.Users.ToList();
            ViewData["users"] = users;

            return View();
        }

        // GET: Administration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Administration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Administration/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Administration/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return View(user);
        }

        // POST: Administration/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, IFormCollection collection)
        {

            var userToDelete = await _userManager.FindByIdAsync(id);
            try
            {
                var result = await _userManager.DeleteAsync(userToDelete);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(userToDelete);
                }
            }
            catch
            {
                return View(userToDelete);
            }
        }

        // Get Administration/EditRoles
        public async Task<ActionResult> EditRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            ViewData["Roles"] = _context.Roles.ToList();

            return View(user);
        }

        //Post Administration/AssignUserToRole
        [HttpPost]
        public async Task<ActionResult> AssignUserToRole(AddRoleToUserDto addRoleToUserDto)
        {
            var user = await _userManager.FindByIdAsync(addRoleToUserDto.UserId);
            var res = await _userManager.AddToRoleAsync(user, addRoleToUserDto.RoleName);

            if (res.Succeeded)
            {
                return View("EditRoles", user);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }
    }
}