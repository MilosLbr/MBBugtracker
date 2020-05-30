using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.ViewModels
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name="Role description")]
        public string RoleDescription { get; set; }
        public IEnumerable<ApplicationUserRole> UserRoles { get; set; }
    }
}
