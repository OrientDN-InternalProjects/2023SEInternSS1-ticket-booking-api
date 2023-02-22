using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketBooking.Service.AircraftService;
using AutoMapper;
using TicketBooking.Data;

namespace TicketBooking.Service
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AircraftDTO, Aircraft>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Model, act => act.MapFrom(src => src.Model))
                .ForMember(dest => dest.Manufacture, act => act.MapFrom(src => src.Manufacturer));
        }
        
    }
}