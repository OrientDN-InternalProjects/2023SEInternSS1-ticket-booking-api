using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
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
        private readonly UserManager<ApplicationUser> userManager;

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

        public async Task<string> RequestBooking(BookingRequestModel model)
        {
            var flight = await flightRepo.GetFlight(model.FlightId);
            var backFlight = await flightRepo.GetFlight(model.RoundFlightId.Value);
            var contact = await contactRepo.GetById(model.ContactId.Value);
            if (flight == null || contact==null)
            {
                return "Please check your data input ";

            }

            var booking = new Booking
            {
                NumberPeople = model.NumberPeople,
                DateBooking = DateTime.Now,
                Reference = model.Reference,
                TotalPrice = 0,
                IsPaid = model.IsPaid,
                IsRoundFlight = model.IsRoundFlight,
                Status = StatusConstant.Active,
                ContactId = contact.Id,
            };
            if (model.UserId != null)
            {
                var user = await userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    return "Invalid User ";
                }
                booking.UserId = model.UserId.ToString();
            }
            await bookingRepo.AddBooking(booking);
            await unitOfWork.CompletedAsync();

            if (booking.IsRoundFlight != true)
            {
                var bookingList = new BookingList
                {
                    NumberSeat = booking.NumberPeople,
                    BookingId = booking.Id,
                    FlightId = model.FlightId,
                    FlightPrice = 0
                };
                if (model.IsBusiness == true)
                {
                    bookingList.FlightPrice = flight.BusinessPrice * bookingList.NumberSeat;
                }
                else
                    bookingList.FlightPrice = flight.EconomyPrice * bookingList.NumberSeat;
                await bookingListRepo.Add(bookingList);
                await unitOfWork.CompletedAsync();
                var list = await ExtraService(bookingList, model.Services);
                bookingListRepo.Update(list);
                booking.TotalPrice += list.FlightPrice;
                booking.BookingLists?.Add(list);
                await unitOfWork.CompletedAsync();
            }
            else if (backFlight!=null)
            {
                var goFlight = new BookingList
                {
                    NumberSeat = booking.NumberPeople,
                    BookingId = booking.Id,
                    FlightId = model.FlightId,
                    FlightPrice = 0
                };
                var roundFlight = new BookingList
                {
                    NumberSeat = booking.NumberPeople,
                    BookingId = booking.Id,
                    FlightId = model.RoundFlightId,
                    FlightPrice = 0
                };
                if (model.IsBusiness == true)
                {
                    goFlight.FlightPrice = flight.BusinessPrice * goFlight.NumberSeat;
                    roundFlight.FlightPrice = flight.BusinessPrice * roundFlight.NumberSeat;
                }
                else
                {
                    goFlight.FlightPrice = flight.EconomyPrice * goFlight.NumberSeat;
                    roundFlight.FlightPrice = flight.EconomyPrice * roundFlight.NumberSeat;
                }
                var goBooking = await ExtraService(goFlight, model.Services);
                var roundBooking = await ExtraService(roundFlight, model.Services);
                booking.BookingLists?.Add(goBooking);
                booking.BookingLists?.Add(roundBooking);
                await bookingListRepo.Add(goBooking);
                await bookingListRepo.Add(roundBooking);
                
                booking.TotalPrice = goBooking.FlightPrice + roundBooking.FlightPrice;

                await unitOfWork.CompletedAsync();
            }
            else
            {
                return "Booking failed, Invalid round trip flight";
            }
            bookingRepo.Update(booking);
            await unitOfWork.CompletedAsync();
            return booking.Id.ToString();

        }

        public async Task<BookingList> ExtraService( BookingList bookingList, List<Guid> extraServices) {
            decimal sum = 0;
            if (extraServices.Count > 0)
            {
                foreach (var service in extraServices)
                {
                    var priceService = await extraServiceRepo.GetById(service);
                    if (priceService != null)
                    {
                        sum += priceService.Price;
                        var bookingService = new BookingExtraService
                        {
                            ExtraServiceId = service,
                            BookingListId = bookingList.Id.Value
                        };
                        await bookingServiceRepo.Add(bookingService);
                        bookingList.BookingServices.Add(bookingService);
                        
                    }
                }
                bookingList.FlightPrice += sum;
            }
            else
                bookingList.FlightPrice += 0;
            return bookingList;
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

    }
}
