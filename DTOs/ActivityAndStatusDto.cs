using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ActivityAndStatusDto
    {
        public TicketStatusDto TicketStatusDto { get; set; }
        public TicketActivityLogDto TicketActivityLogDto { get; set; }
    }
}
