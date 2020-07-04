using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; } // ProjectOwner

        public virtual ICollection<Ticket> ProjectTickets { get; set; }
        public virtual ICollection<ProjectsAndUsers> ProjectsAndUsers { get; set; }

    }
}
