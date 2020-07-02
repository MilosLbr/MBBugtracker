using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ProjectsAndUsersDto
    {
        public int ProjectId { get; set; }

        public string  ApplicationUserId { get; set; }

        public ApplicationUserBasicInfoDto ApplicationUser { get; set; }
    }
}
