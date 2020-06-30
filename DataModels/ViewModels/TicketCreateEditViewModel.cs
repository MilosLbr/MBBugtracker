
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataModels.ViewModels
{
    public class TicketCreateEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        
        [Required]
        [Display(Name="Project")]
        public int ProjectId { get; set; }
        public IEnumerable<Project> AllProjects { get; set; }
        
        [Required]
        [Display(Name = "Priority")]
        public int TicketPriorityId { get; set; }
        public IEnumerable<TicketPriority> AllTicketPriorities { get; set; }
        
        [Required]
        [Display(Name = "Status")]
        public string TicketStatusId { get; set; }
        public IEnumerable<TicketStatus> AllTicketStatuses { get; set; }

        [Required]
        [Display(Name ="Type")]
        public string TicketTypeId { get; set; }
        public IEnumerable<TicketType> AllTicketTypes { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime UpdatedOn { get; set; }
        [Required]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; } = DateTime.Now;

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } 

        
        [Required]
        [Display(Name = "Assigned to")]
        public string AssignedTo { get; set; }
        public IEnumerable<ApplicationUser> AllAppUsers { get; set; }


    }
}
