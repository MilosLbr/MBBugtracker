using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataModels
{
    public class Ticket 
    {
        // title, desc, submiter, project, priority, status, type, created updated

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime DueDate { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; } // created by
        public string ApplicationUserId { get; set; }

        public string UpdatedByUserId { get; set; }
        public string AssignedTo { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public int TicketPriorityId { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }

        public int TicketStatusId { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }

        public int TicketTypeId { get; set; }
        public virtual TicketType TicketType { get; set; }

        public virtual TicketResolution TicketResolution { get; set; }

    }
}
