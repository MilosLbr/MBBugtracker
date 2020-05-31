using AutoMapper;
using DataModels;
using DataModels.ViewModels;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers.AutoMapper
{
    public class MyMapperProfile : Profile
    {
        public MyMapperProfile()
        {
            //Ticket entity mappings
            CreateMap<Ticket, TicketCreateEditViewModel>().ReverseMap();
            CreateMap<Ticket, TicketListViewModel>();
            CreateMap<Ticket, TicketDetailsViewModel>();

            //AppUser mappings
            CreateMap<ApplicationUser, ApplicationUserBasicInfoDto>();
            CreateMap<ApplicationUser, ApplicationUserViewModel>();
            CreateMap<ApplicationUser, ApplicationUserEditRolesViewModel>();

            //AppRole mappings
            CreateMap<ApplicationRole, ApplicationRoleViewModel>().ReverseMap();

            //Projects mappings
            CreateMap<ProjectCreateDto, Project>();
            CreateMap<Project, ProjectEditViewModel>().ReverseMap();

            CreateMap<Project, ProjectDetailsViewModel>()
                .ForMember(vm => vm.AssignedDevelopers, opt => opt.MapFrom(prop => prop.ProjectsAndUsers));
            CreateMap<ProjectsAndUsers, ProjectsAndUsersViewModel>();
        }
    }
}
