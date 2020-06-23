using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class TicketResolution
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; } // username
        public string CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public string ResolutionComment { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
