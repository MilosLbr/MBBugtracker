using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ViewModels
{
    public class ApplicationUserEditRolesViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public IEnumerable<ApplicationUserRole> UserRoles { get; set; }
        public IEnumerable<ApplicationRole> AllAvailableRoles { get; set; }
    }
}
