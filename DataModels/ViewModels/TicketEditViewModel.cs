using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.ViewModels
{
    public class TicketEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Required]
        public string Priority { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime UpdatedOn { get; set; }
        public IEnumerable<ApplicationUser> AllAppUsers { get; set; }

        [Display(Name ="Assigned to")]
        public string AssignedTo { get; set; }
    }
}
