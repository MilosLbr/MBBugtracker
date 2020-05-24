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
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        [Display(Name ="Created on")]
        public DateTime CreatedOn { get; set; }
        [Display(Name = "Updated on")]
        public DateTime UpdatedOn { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public string UpdatedByUserId { get; set; }

        [Display(Name ="Updated by")]
        public string UpdatedByUserName { get; set; }
        public string AssignedTo { get; set; }
    }
}
