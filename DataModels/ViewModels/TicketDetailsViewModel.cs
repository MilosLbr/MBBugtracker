using DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.ViewModels
{
    public class TicketDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Priority")]
        public TicketPriority TicketPriority { get; set; }
        [Display(Name = "Status")]
        public TicketStatus TicketStatus { get; set; }
        [Display(Name = "Type")]
        public TicketType TicketType { get; set; }
        public ProjectDetailsViewModel Project { get; set; }


        public string UpdatedByUserId { get; set; }

        [Display(Name = "Updated by")]
        public string UpdatedByUserName { get; set; }

        public ApplicationUser ApplicationUser { get; set; } //created by

        [Display(Name = "Assigned to")]
        public string AssignedTo { get; set; }

        public TicketResolution TicketResolution { get; set; }

        public IEnumerable<TicketCommentListDto> TicketComments { get; set; }
        public IEnumerable<TicketStatus> TicketStatuses { get; set; }
        public IEnumerable<TicketActivityLogDto> TicketActivityLogs { get; set; }
    }
}
