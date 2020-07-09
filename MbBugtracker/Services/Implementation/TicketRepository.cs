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
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext context) : base(context) 
        {
        }

        public async Task<IEnumerable<Ticket>> GetTicketsForCurrentUser(ApplicationUser currentUser)
        {
            // get tickets this user has created or this user is assigned to
            var userTickets = await this.Filter(t => t.ApplicationUserId == currentUser.Id || t.AssignedTo == currentUser.UserName).OrderByDescending(t => t.DueDate).ToListAsync();

            return userTickets;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsForCurrentUsersProjects(ApplicationUser currentUser)
        {
            // get tickets this user has created or this user is assigned to, or tickets from the projects the user is assigned to

            var usersProjectTickets = await this
                .Filter(t => t.ApplicationUserId == currentUser.Id 
                || t.AssignedTo == currentUser.UserName 
                || t.Project.ProjectsAndUsers.Any(pau => pau.ApplicationUserId == currentUser.Id))
                .ToListAsync();

            return usersProjectTickets;
        }
    }
}
