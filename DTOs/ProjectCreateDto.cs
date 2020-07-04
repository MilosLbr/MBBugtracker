using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ProjectCreateDto
    {        
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public List<string> SelectedUserIds { get; set; }
    }
}
