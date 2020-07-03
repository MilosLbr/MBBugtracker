using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class TicketActivityLogDto
    {
        public int Id { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActivityDescription { get; set; }
        public int TicketId { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUserBasicInfoDto ApplicationUser { get; set; }
    }
}
