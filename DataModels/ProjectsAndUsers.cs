using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class ProjectsAndUsers
    {
        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

    }
}
