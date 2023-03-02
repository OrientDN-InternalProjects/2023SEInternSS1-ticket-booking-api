﻿using AutoMapper;
using TicketBooking.Common.Constant;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Model.DataModel;
using TicketBooking.Service.Models;

namespace TicketBooking.Service.Services.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository bookingRepo;
        private readonly IBookingListRepository bookingListRepo;
        private readonly IBookingSeatRepository bookingSeatRepo;
        private readonly IBookingServiceRepository bookingServiceRepo;
        private readonly IPassengerRepository passengerRepo;
        private readonly ITicketRepository ticketRepo;
        private readonly IContactDetailRepository contactRepo;
        private IUnitOfWork unitOfWork;
        private readonly IFlightRepository flightRepo;
        private readonly IExtraServiceRepository extraServiceRepo;
        private readonly IMapper mapper;

        public BookingService(IBookingRepository booking
            , IUnitOfWork unitOfWork
            , IMapper mapper
            , ITicketRepository tickeetRepo
            , IPassengerRepository passenger
            , IBookingSeatRepository bookingSeat
            , IBookingListRepository bookingList
            , IContactDetailRepository contactRepo
            , IFlightRepository flightRepo
            , IBookingServiceRepository serviceRepository
            , IExtraServiceRepository extra)
        {
            bookingRepo = booking;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.ticketRepo = tickeetRepo;
            bookingListRepo = bookingList;
            bookingSeatRepo = bookingSeat;
            passengerRepo = passenger;
            this.contactRepo = contactRepo;
            this.flightRepo = flightRepo;
            bookingServiceRepo = serviceRepository;
            extraServiceRepo = extra;
        }

        public async Task<Response> RequestBooking(BookingRequestModel model)
        {
            var booking = new Booking
            {
                NumberPeople = model.NumberPeople,
                DateBooking = DateTime.Now,
                Reference = model.Reference,
                TotalPrice = 0,
                IsPaid = model.IsPaid,
                IsRoundFlight = model.IsRoundFlight,
                Status = StatusConstant.Active,
                ContactId = model.ContactId.Value,
            };
            if (model.UserId != null)
            {
                booking.UserId = model.UserId.ToString();
            }
            await bookingRepo.AddBooking(booking);
            var flight = await flightRepo.GetFlight(model.FlightId);
            if (flight != null)
            {
                if (booking.IsRoundFlight != true)
                {
                    var bookingListVM = new BookingListViewModel
                    {

                        NumberSeat = booking.NumberPeople,
                        BookingId = booking.Id,
                        FlightId = model.FlightId,
                        FlightPrice = 0
                    };
                    if (model.IsBusiness == true)
                    {
                        bookingListVM.FlightPrice = (decimal)(flight.BusinessPrice * bookingListVM.NumberSeat);
                    }
                    else
                        bookingListVM.FlightPrice = (decimal)(flight.EconomyPrice * bookingListVM.NumberSeat);
                    var bookingList = mapper.Map<BookingList>(bookingListVM);
                    await bookingListRepo.Add(bookingList);
                    booking.TotalPrice = bookingList.FlightPrice;
                }
                else if (model.RoundFlightId != null)
                {
                    var goFlight = new BookingListViewModel
                    {
                        NumberSeat = booking.NumberPeople,
                        BookingId = booking.Id,
                        FlightId = model.FlightId,
                        FlightPrice = 0
                    };
                    var roundFlight = new BookingListViewModel
                    {
                        NumberSeat = booking.NumberPeople,
                        BookingId = booking.Id,
                        FlightId = model.RoundFlightId,
                        FlightPrice = 0
                    };
                    if (model.IsBusiness == true)
                    {
                        goFlight.FlightPrice = (decimal)(flight.BusinessPrice * goFlight.NumberSeat);
                        roundFlight.FlightPrice = (decimal)(flight.BusinessPrice * roundFlight.NumberSeat);
                    }
                    else
                    {
                        goFlight.FlightPrice = (decimal)(flight.EconomyPrice * goFlight.NumberSeat);
                        roundFlight.FlightPrice = (decimal)(flight.EconomyPrice * roundFlight.NumberSeat);
                    }
                    booking.TotalPrice = 0 + (goFlight.FlightPrice + roundFlight.FlightPrice);
                    var bookingGoFlight = mapper.Map<BookingList>(goFlight);
                    var bookingRoundFlight = mapper.Map<BookingList>(roundFlight);
                    booking.BookingLists?.Add(bookingGoFlight);
                    booking.BookingLists?.Add(bookingRoundFlight);
                    await bookingListRepo.Add(bookingGoFlight);
                    await bookingListRepo.Add(bookingRoundFlight);
                    await unitOfWork.CompletedAsync();
                }
                else
                {
                    return new Response
                    {
                        Status = true,
                        Message = "Booking failed, Invalid round trip flight",
                        Data = booking
                    };
                }
                await bookingRepo.AddBooking(booking);
                await unitOfWork.CompletedAsync();
                return new Response
                {
                    Status = true,
                    Message = "Booking success",
                    Data = booking
                };
            }
            return new Response
            {
                Status = true,
                Message = "Invalid flight ",
                Data = null
            };


        }

        public async Task<Response> CancelBooking(Guid bookingId)
        {
            var booking = await bookingRepo.GetById(bookingId);
            if (booking != null)
            {
                booking.Status = StatusConstant.Cancel;
                bookingRepo.Update(booking);
                await unitOfWork.CompletedAsync();
                return new Response
                {
                    Status = true,
                    Message = "Cancel successfully",
                    Data = booking
                };
            }
            return new Response
            {
                Status = true,
                Message = "Cancel failed!! Invalid booking"

            };
        }

        public async Task<Response> AddBookingService(List<Guid> extraServices, Guid bookingList)
        {
            var checkBooking = await bookingListRepo.GetById(bookingList);
            decimal sum = 0;
            if (checkBooking != null)
            {
                if (extraServices.Count > 0)
                {
                    for (int i = 0; i < extraServices.Count; i++)
                    {
                        ExtraService priceService = await extraServiceRepo.GetById(extraServices[i]);
                        sum += priceService.Price;
                        var bookingService = new BookingExtraService
                        {
                            ExtraServiceId = extraServices[i],
                            BookingListId = bookingList
                        };
                        await bookingServiceRepo.Add(bookingService);
                    }
                    checkBooking.FlightPrice += sum;
                    bookingListRepo.Update(checkBooking);
                    var booking = await bookingRepo.GetById((Guid)checkBooking.BookingId);
                    if (booking != null)
                    {
                        booking.TotalPrice = 0;
                        var list = bookingListRepo.GetBookingList(booking.Id);
                        foreach (var item in list)
                        {
                            booking.TotalPrice += item.FlightPrice;
                        }
                        bookingRepo.Update(booking);
                    }

                    await unitOfWork.CompletedAsync();
                }
                else
                    checkBooking.FlightPrice += 0;
                return new Response
                {
                    Status = true,
                    Message = "Add extra successfull",
                    Data = checkBooking
                };
            }
            return new Response
            {
                Status = false,
                Message = "Add service failed!! Invalid booking"
            };
        }

    }
}