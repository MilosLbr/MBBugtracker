using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class TicketType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public virtual IEnumerable<Ticket> Tickets { get; set; }
    }
}
