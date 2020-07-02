using DataModels;
using MbBugtracker.Data;
using MbBugtracker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MbBugtracker.Services.Implementation
{
    public class TicketPriorityRepository : Repository<TicketPriority>, ITicketPriorityRepository
    {
        public TicketPriorityRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
