using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class TicketResolutionDto
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public string ResolutionComment { get; set; }

        public int TicketId { get; set; }
        public virtual TicketBasicInfoDto Ticket { get; set; }
    }
}
