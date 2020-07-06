using DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Project name")]
        public string ProjectName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUserBasicInfoDto ApplicationUser { get; set; } // ProjectOwner
        public int ProjectStatusId { get; set; }
        public virtual ProjectStatusDto ProjectStatus { get; set; }

        public IEnumerable<TicketListDto> ProjectTickets { get; set; }
        public IEnumerable<ProjectsAndUsersViewModel> AssignedDevelopers { get; set; }
    }
}
