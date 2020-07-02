using DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.ViewModels
{
    public class ProjectEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Project name")]
        public string ProjectName { get; set; }
        [Required]
        public string Description { get; set; }


        [Required]
        public List<string> SelectedUserIds { get; set; }
        public virtual IEnumerable<ProjectsAndUsersDto> ProjectsAndUsers { get; set; }
    }
}
