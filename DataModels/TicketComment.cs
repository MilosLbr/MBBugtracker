using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class TicketComment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateAdded { get; set; }
        public string CreatedBy { get; set; } // username
        public string CreatedByUserId { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

    }
}
