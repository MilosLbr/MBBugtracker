using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class ApplicationRole : IdentityRole
    {
        public string RoleDescription { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
