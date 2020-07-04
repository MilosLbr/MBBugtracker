using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class ApplicationUser : IdentityUser
    {
        public string CustomTag { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<ProjectsAndUsers> ProjectsAndUsers { get; set; }
        public virtual ICollection<TicketActivityLog> TicketActivityLogs { get; set; }

    }
}
