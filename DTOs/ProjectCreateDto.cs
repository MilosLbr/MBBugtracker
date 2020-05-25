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
        public List<string> SelectedUserIds { get; set; }
    }
}
