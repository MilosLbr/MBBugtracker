using DataModels;
using MbBugtracker.Data;
using MbBugtracker.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MbBugtracker.Services.Implementation
{
    public class TicketActivityLogRepository : Repository<TicketActivityLog>, ITicketActivityLogRepository
    {
        public TicketActivityLogRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
