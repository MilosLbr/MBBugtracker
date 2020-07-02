using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataModels;
using DataModels.ViewModels;
using DTOs;
using MbBugtracker.Data;
using MbBugtracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MbBugtracker.Controllers
{
    [Authorize(Roles ="Admin,ProjectManager")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AdministrationController( ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: Administration
        public async Task<ActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersViewModel = _mapper.Map<IEnumerable<ApplicationUserViewModel>>(users);

            return View(usersViewModel);
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
            var allRoles = await _roleManager.Roles.ToListAsync();

            var userViewModel = _mapper.Map<ApplicationUserEditRolesViewModel>(user);
            userViewModel.AllAvailableRoles = allRoles;

            return View(userViewModel);
        }

        //Post Administration/AssignUserToRole
        [HttpPost]
        public async Task<ActionResult> AssignUserToRole(AddRemoveRoleForUserDto addRoleToUserDto)
        {
            var user = await _userManager.FindByIdAsync(addRoleToUserDto.UserId);

            if(await _userManager.IsInRoleAsync(user, addRoleToUserDto.RoleName))
            {
                return BadRequest("The user is already in this role!");
            }

            var res = await _userManager.AddToRoleAsync(user, addRoleToUserDto.RoleName);

            if (res.Succeeded)
            {
                return Ok("Added role " + addRoleToUserDto.RoleName + " to " + user.UserName + "!");
            }
            else
            {
                return BadRequest("An error happened while adding the user to role!");
            }

        }

        //Post Administration/RemoveUserFromRole
        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(AddRemoveRoleForUserDto removeRoleForUserDto)
        {
            var user = await _userManager.FindByIdAsync(removeRoleForUserDto.UserId);

            var res = await _userManager.RemoveFromRoleAsync(user, removeRoleForUserDto.RoleName);

            if (res.Succeeded)
            {
                return Ok("Removed role " + removeRoleForUserDto.RoleName + " from this user!");
            }
            else
            {
                return BadRequest("An error happened while trying to remove the role from user!");
            }
        }
    }
}