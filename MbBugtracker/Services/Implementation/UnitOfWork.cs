using DataModels;
using MbBugtracker.Data;
using MbBugtracker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MbBugtracker.Services.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ITicketRepository Tickets { get; set; }
        public ITicketStatusRepository TicketStatuses { get; set; }
        public ITicketResolutionRepository TicketResolutions { get; set; }
        public ITicketCommentRepository TicketComments { get; set; }
        public ITicketPriorityRepository TicketPriorities { get; set; }
        public ITicketTypeRepository TicketTypes { get; set; }
        public IProjectRepository Projects { get; set; }
        public ITicketActivityLogRepository TicketActivityLogs { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Tickets = new TicketRepository(_context);
            TicketStatuses = new TicketStatusRepository(_context);
            TicketResolutions = new TicketResolutionRepository(_context);
            TicketComments = new TicketCommentRepository(_context);
            TicketPriorities = new TicketPriorityRepository(_context);
            TicketTypes = new TicketTypeRepository(_context);
            Projects = new ProjectRepository(_context);
            TicketActivityLogs = new TicketActivityLogRepository(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
