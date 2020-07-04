using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class ProjectStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
