using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class TicketBasicInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime DueDate { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public int TicketTypeId { get; set; }
        public ProjectBasicInfoDto Project { get; set; }


        public string UpdatedByUserId { get; set; }
        public string UpdatedByUserName { get; set; }

        public ApplicationUserBasicInfoDto ApplicationUser { get; set; } //created by

        public string AssignedTo { get; set; }
    }
}
