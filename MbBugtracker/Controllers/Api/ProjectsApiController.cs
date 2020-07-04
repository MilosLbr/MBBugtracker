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
using MbBugtracker.Services.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectsApiController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
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
            projectToCreate.StartDate = DateTime.Now;

            _unitOfWork.Projects.Add(projectToCreate);
            
            if (await _unitOfWork.Complete() >=1)
            {
                return Ok("Created!");
            }
            else
            {
                return BadRequest("An error happened while creating new project!");
            }
        }
    }
}