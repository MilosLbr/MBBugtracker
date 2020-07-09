using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MbBugtracker.Services.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetTicketsForCurrentUser(ApplicationUser currentUser);
        Task<IEnumerable<Ticket>> GetTicketsForCurrentUsersProjects(ApplicationUser currentUser);
    }
}
