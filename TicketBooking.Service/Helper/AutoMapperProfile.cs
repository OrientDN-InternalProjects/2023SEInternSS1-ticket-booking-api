using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TicketBooking.Data;
using TicketBooking.Data.DataModel;
using TicketBooking.Model.Models;
using TicketBooking.Service.Models;

namespace TicketBooking.Service.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AircraftViewModel, Aircraft>().ReverseMap()
            .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
            .ForMember(dest => dest.Model, act => act.MapFrom(src => src.Model))
            .ForMember(dest => dest.Manufacture, act => act.MapFrom(src => src.Manufacture));

            CreateMap<Booking, BookingRequestModel>().ReverseMap()
            .ForMember(dest => dest.NumberPeople, act => act.MapFrom(src => src.NumberPeople))
            .ForMember(dest => dest.DateBooking, act => act.MapFrom(src => src.DateBooking))
            .ForMember(dest => dest.Reference, act => act.MapFrom(src => src.Reference))
            .ForMember(dest => dest.TotalPrice, act => act.MapFrom(src => src.TotalPrice))
            .ForMember(dest => dest.IsPaid, act => act.MapFrom(src => src.IsPaid))
            .ForMember(dest => dest.IsRoundFlight, act => act.MapFrom(src => src.IsRoundFlight))
            .ForMember(dest => dest.ContactId, act => act.MapFrom(src => src.ContactId))
            .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Status, act => act.MapFrom(src => src.Status));

            CreateMap<ContactViewModel, ContactDetail>().ReverseMap()
               .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
               .ForMember(dest => dest.MiddleName, act => act.MapFrom(src => src.MiddleName))
               .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
               .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
               .ForMember(dest => dest.PhoneNumber, act => act.MapFrom(src => src.PhoneNumber));
            CreateMap<BookingListViewModel, BookingList>().ReverseMap();

        }

    }
}