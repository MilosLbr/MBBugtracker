using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels.ViewModels
{
    public class MyDashboardViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public IEnumerable<ProjectDetailsDto> MyProjects { get; set; }
        public IEnumerable<TicketBasicInfoDto> MyTickets { get; set; }

        public IEnumerable<TicketBasicInfoDto> MyTicketsDueToday { get; set; }
        public IEnumerable<TicketBasicInfoDto> MyOverdueTickets { get; set; }

    }
}
