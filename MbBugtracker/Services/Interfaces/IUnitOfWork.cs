using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MbBugtracker.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ITicketRepository Tickets { get; }
        ITicketStatusRepository TicketStatuses { get; }
        ITicketResolutionRepository TicketResolutions { get; }
        ITicketCommentRepository TicketComments { get; }
        ITicketPriorityRepository TicketPriorities { get; }
        ITicketTypeRepository TicketTypes { get; }
        IProjectRepository Projects { get; }
        ITicketActivityLogRepository TicketActivityLogs { get; }
        
        Task<int> Complete();
    }
}
