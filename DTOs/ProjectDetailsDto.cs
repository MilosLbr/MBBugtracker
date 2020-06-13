using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ProjectDetailsDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }

        public IEnumerable<TicketListDto> ProjectTickets { get; set; }
        public IEnumerable<ProjectsAndUsersDto> AssignedDevelopers { get; set; }
    }
}
