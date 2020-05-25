using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.ViewModels
{
    public class ProjectCreateViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string Description { get; set; }

        public List<ApplicationUser> AvailableAppUsers { get; set; }
        public List<string> SelectedUserIds { get; set; }
    }
}
