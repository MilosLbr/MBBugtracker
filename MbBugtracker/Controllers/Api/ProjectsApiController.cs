using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels.ViewModels;
using DTOs;
using MbBugtracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MbBugtracker.Controllers.Api
{
    [Route("api/projects")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class ProjectsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNewProject(ProjectCreateDto projectCreateDto)
        {
            var projects = await _context.Projects.ToListAsync();
            return Ok();
        }
    }
}