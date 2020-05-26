using DTOs;

namespace DataModels.ViewModels
{
    public class ProjectsAndUsersViewModel
    {
        public int ProjectId { get; set; }

        public virtual ApplicationUserBasicInfoDto ApplicationUser { get; set; }
    }
}