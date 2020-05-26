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

        public IEnumerable<Ticket> ProjectTickets { get; set; }
        public IEnumerable<ProjectsAndUsersViewModel> AssignedDevelopers { get; set; }
    }
}
