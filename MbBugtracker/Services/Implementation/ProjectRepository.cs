using DataModels;
using MbBugtracker.Data;
using MbBugtracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MbBugtracker.Services.Implementation
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetProjectsForCurrentUser(ApplicationUser currentUser)
        {
            // return the project current user has created, or projects current user is assigned to work on

            var userProjects = await this
                .Filter(p => p.ApplicationUserId == currentUser.Id 
                || p.ProjectsAndUsers.Any(pau => pau.ApplicationUserId == currentUser.Id))
                .ToListAsync();

            return userProjects;
        }
    }
}
