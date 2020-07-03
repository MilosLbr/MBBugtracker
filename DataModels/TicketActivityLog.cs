using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class TicketActivityLog
    {
        public int Id { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActivityDescription { get; set; }

        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
