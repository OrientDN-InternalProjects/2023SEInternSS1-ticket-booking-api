using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TicketBooking.Data.DataModel;
using TicketBooking.Model.Models;

namespace TicketBooking.Service
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AircraftViewModel, Aircraft>().ReverseMap()
            .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
            .ForMember(dest => dest.Model, act => act.MapFrom(src => src.Model))
            .ForMember(dest => dest.Manufacture, act => act.MapFrom(src => src.Manufacture));

            CreateMap<FlightViewModel, Flight>().ReverseMap();

            CreateMap<FlightScheViewModel, FlightSchedule>().ReverseMap();

            CreateMap<AirportViewModel, Airport>().ReverseMap();
        }
        
    }
}