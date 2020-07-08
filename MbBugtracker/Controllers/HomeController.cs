using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MbBugtracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DataModels;
using DataModels.ViewModels;
using MbBugtracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DTOs;
using DataModels.EnumConstants;
using Microsoft.VisualBasic;

namespace MbBugtracker.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if(currentUser != null)
            {
                var myDashboardViewModel = await PrepareDashboardViewModel(currentUser);

                return View(myDashboardViewModel);
            }
            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(HomeLoginViewModel loginModel, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View("Index");
                }
            }

            // If we got this far, something failed, redisplay form
            return View("Index");
        }

        private async Task<MyDashboardViewModel> PrepareDashboardViewModel(ApplicationUser currentUser)
        {
            // get projects user created or the user is assigned to
            var myProjects = await _unitOfWork.Projects.Filter(p => p.ApplicationUserId == currentUser.Id || p.ProjectsAndUsers.Where(pau => pau.ApplicationUserId == currentUser.Id).Count() > 0).ToListAsync();

            var myProjectsDto = _mapper.Map<IEnumerable<ProjectDetailsDto>>(myProjects);

            // get tickets user created or the user is assigned to
            var myTickets = await _unitOfWork.Tickets.Filter(t => t.ApplicationUserId == currentUser.Id || t.AssignedTo == currentUser.UserName).ToListAsync();

            var myTicketsDto = _mapper.Map<IEnumerable<TicketBasicInfoDto>>(myTickets);

            // tickets due on this date
            var myTicketsDueToday = myTickets.Where(t => {
                var now = DateTime.Now;
                var result = DateTime.Compare(now.Date, t.DueDate.Date);

                return result == 0 && t.TicketStatusId != (int)EnumConstants.TicketStatuses.Closed; // get only tickets that are not closed (status 2)
            });
            var myTicketsDueTodayDto = _mapper.Map<IEnumerable<TicketBasicInfoDto>>(myTicketsDueToday);

            // overdue tickets
            var myOverdueTickets = myTickets.Where(t =>
            {
                var now = DateTime.Now;
                var result = DateTime.Compare(now.Date, t.DueDate.Date);

                return result > 0 && t.TicketStatus.Id != (int)EnumConstants.TicketStatuses.Closed; 
            });
            var myOverdueTicketsDto = _mapper.Map<IEnumerable<TicketBasicInfoDto>>(myOverdueTickets);

            var myDashboardViewModel = new MyDashboardViewModel()
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                MyProjects = myProjectsDto,
                MyTickets = myTicketsDto,
                MyTicketsDueToday = myTicketsDueTodayDto,
                MyOverdueTickets = myOverdueTicketsDto
            };

            return myDashboardViewModel;
        }
    }
}
