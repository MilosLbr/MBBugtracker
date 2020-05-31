﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataModels.ViewModels
{
    public class TicketEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Project")]
        public int ProjectId { get; set; }
        public IEnumerable<Project> AllProjects { get; set; }
        
        [Required]
        public int TicketPriorityId { get; set; }
        public TicketPriority TicketPriority { get; set; }
        public IEnumerable<TicketPriority> AllTicketPriorities { get; set; }
        
        [Required]
        public string Status { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime UpdatedOn { get; set; }
        
        

        [Display(Name ="Assigned to")]
        [Required]
        public string AssignedTo { get; set; }
        public IEnumerable<ApplicationUser> AllAppUsers { get; set; }
    }
}
