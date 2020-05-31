using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class TicketPriority
    {
        public int Id { get; set; }
        public string PriorityName { get; set; }

        public virtual IEnumerable<Ticket> Tickets { get; set; }

    }
}
