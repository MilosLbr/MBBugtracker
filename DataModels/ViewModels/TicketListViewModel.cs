using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataModels.ViewModels
{
    public class TicketListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectName { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }


        public string UpdatedByUserId { get; set; }

        [Display(Name ="Updated by")]
        public string UpdatedByUserName { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Assigned to")]
        public string AssignedTo { get; set; }
        public Project Project { get; set; }
    }
}
