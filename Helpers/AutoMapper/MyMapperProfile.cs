using AutoMapper;
using DataModels;
using DataModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers.AutoMapper
{
    public class MyMapperProfile : Profile
    {
        public MyMapperProfile()
        {
            CreateMap<Ticket, TicketCreateViewModel>();
            CreateMap<TicketCreateViewModel, Ticket>();

            CreateMap<Ticket, TicketEditViewModel>().ReverseMap();
        }
    }
}
