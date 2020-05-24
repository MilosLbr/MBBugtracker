using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataModels
{
    public class Ticket 
    {
        // title, desc, submiter, project, priority, status, type, created updated

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectName { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string AssignedTo { get; set; }

    }
}
