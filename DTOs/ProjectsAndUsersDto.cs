using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ProjectsAndUsersDto
    {
        public int ProjectId { get; set; }

        public virtual ApplicationUserBasicInfoDto ApplicationUser { get; set; }
    }
}
