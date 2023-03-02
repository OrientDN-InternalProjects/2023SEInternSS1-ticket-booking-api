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

        public async Task<Response> RequestBooking(BookingRequestModel model)
        {
            var flight = await flightRepo.GetFlight(model.FlightId);
            var backFlight = await flightRepo.GetFlight(model.RoundFlightId.Value);
            var contact = await contactRepo.GetById(model.ContactId.Value);
            if (flight == null || contact==null)
            {
                return new Response
                {
                    Status = false,
                    Message = "Please check your data input "
                };
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
                    return new Response
                    {
                        Status = false,
                        Message = "Invalid User "
                    };
                }
                booking.UserId = model.UserId.ToString();
            }
            await bookingRepo.AddBooking(booking);

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
                    bookingListVM.FlightPrice = flight.BusinessPrice * bookingListVM.NumberSeat;
                }
                else
                    bookingListVM.FlightPrice = flight.EconomyPrice * bookingListVM.NumberSeat;
                var bookingList = mapper.Map<BookingList>(bookingListVM);
                await ExtraService(bookingList, model.Services);
                await bookingListRepo.Add(bookingList);
                booking.TotalPrice = bookingList.FlightPrice;
            }
            else if (backFlight!=null)
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
                    goFlight.FlightPrice = flight.BusinessPrice * goFlight.NumberSeat;
                    roundFlight.FlightPrice = flight.BusinessPrice * roundFlight.NumberSeat;
                }
                else
                {
                    goFlight.FlightPrice = flight.EconomyPrice * goFlight.NumberSeat;
                    roundFlight.FlightPrice = flight.EconomyPrice * roundFlight.NumberSeat;
                }
                var bookingGoFlight = mapper.Map<BookingList>(goFlight);
                var bookingRoundFlight = mapper.Map<BookingList>(roundFlight);
                await ExtraService(bookingGoFlight, model.Services);
                await ExtraService(bookingRoundFlight, model.Services);

                booking.TotalPrice += bookingGoFlight.FlightPrice + bookingRoundFlight.FlightPrice;
                
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
                    Data = booking.Id
                };
            }
            await bookingRepo.AddBooking(booking);
            await unitOfWork.CompletedAsync();
            return new Response
            {
                Status = true,
                Message = "Booking success",
                Data = booking.Id
            };

        }

        public void AddTicket(ref Booking booking)
        {

        }

        public async Task ExtraService(BookingList bookingList, List<Guid> extraServices) {
            decimal sum = 0;
            if (extraServices.Count > 0)
            {
                for (int i = 0; i < extraServices.Count; i++)
                {
                    var priceService =await extraServiceRepo.GetById(extraServices[i]);
                    if (priceService != null)
                    {
                        sum += priceService.Price;
                        var bookingService = new BookingExtraService
                        {
                            ExtraServiceId = extraServices[i],
                            BookingListId = bookingList.Id.Value
                        };
                        await bookingServiceRepo.Add(bookingService);
                    }
                }
                bookingList.FlightPrice += sum;
            }
            else
                bookingList.FlightPrice += 0;
            bookingListRepo.Update(bookingList);
            await unitOfWork.CompletedAsync();
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
                    var booking = await bookingRepo.GetById(checkBooking.BookingId.Value);
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
