using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Internal;
using DataModels;
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
    public class ProjectsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectsApiController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewProject(ProjectCreateDto projectCreateDto)
        {
            if(string.IsNullOrWhiteSpace(projectCreateDto.Description) || string.IsNullOrWhiteSpace(projectCreateDto.ProjectName) || projectCreateDto.SelectedUserIds.Count == 0)
            {
                return BadRequest("All fields are required!");
            }
            var projectToCreate = _mapper.Map<Project>(projectCreateDto);
            var projectUsers = new List<ProjectsAndUsers>();

            foreach (var userId in projectCreateDto.SelectedUserIds)
            {
                var projectUser = new ProjectsAndUsers
                {
                    ApplicationUserId = userId,
                };

                projectUsers.Add(projectUser);
            }
            projectToCreate.ProjectsAndUsers = projectUsers;


            await _context.Projects.AddAsync(projectToCreate);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}