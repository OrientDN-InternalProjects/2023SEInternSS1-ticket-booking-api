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

            CreateMap<Airport, AirportViewModel>().ReverseMap()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                .ForMember(dest => dest.Code, act => act.MapFrom(src => src.Code))
                .ForMember(dest => dest.Country, act => act.MapFrom(src => src.Country));

            CreateMap<FlightViewModel, Flight>().ReverseMap();

            CreateMap<Flight, FlightViewModel>()
                .ForMember(dest => dest.DepartTime, act => act.MapFrom(src => src.Schedule.DepartureTime.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(dest => dest.ArrivalTime, act => act.MapFrom(src => src.Schedule.ArrivalTime.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(dest => dest.DepartAirport, act => act.MapFrom(src => src.Schedule.AirportDepart.Code))
                .ForMember(dest => dest.ArrivalAirport, act => act.MapFrom(src => src.Schedule.AirportArrival.Code));

            CreateMap<FlightUpdateModel, Flight>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.AircraftId, act => act.MapFrom(src => src.AircraftId))
                .ForMember(dest => dest.DefaultBaggage, act => act.MapFrom(src => src.DefaultBaggage))
                .ForMember(dest => dest.EconomyPrice, act => act.MapFrom(src => src.EconomyPrice))
                .ForMember(dest => dest.BusinessPrice, act => act.MapFrom(src => src.BusinessPrice))
                .ForPath(dest => dest.Schedule.DepartureTime, act => act.MapFrom(src => src.FlightSche.DepartureTime))
                .ForPath(dest => dest.Schedule.ArrivalTime, act => act.MapFrom(src => src.FlightSche.ArrivalTime))
                .ForPath(dest => dest.Schedule.DepartureTime, act => act.MapFrom(src => src.FlightSche.DepartureTime))
                .ForPath(dest => dest.Schedule.ArrivalTime, act => act.MapFrom(src => src.FlightSche.ArrivalTime));
            


            CreateMap<FlightScheViewModel, FlightSchedule>()
            .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
            .ForMember(dest => dest.DepartureAirportId, act => act.MapFrom(src => src.DepartureAirpotId))
            .ForMember(dest => dest.ArrivalAirportId, act => act.MapFrom(src => src.ArrivalAirportId));

            CreateMap<AirportViewModel, Airport>().ReverseMap();
        }
        
    }
}