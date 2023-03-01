using AutoMapper;
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

        public void InsertPassenger(int numPeople, Guid bookingId)
        {
            for (int i = 0; i < numPeople; i++)
            {
                var passenger = new Passenger { BookingId = bookingId };
                var ticket = new Ticket { BookingId = bookingId };
                passengerRepo.AddPassenger(passenger);
                ticketRepo.AddTicket(ticket);
            }
            unitOfWork.CompletedAsync();
        }

        public async Task<Response> RequestBooking(BookingRequestModel model)
        {
            try
            {
                var booking = new Booking
                {
                    NumberPeople = model.NumberPeople,
                    DateBooking = DateTime.Now,
                    Reference = model.Reference,
                    TotalPrice = 0,
                    IsPaid = model.IsPaid,
                    IsRoundFlight = model.IsRoundFlight,
                    Status = model.Status,

                };
                await bookingRepo.AddBooking(booking);
                var flight = await flightRepo.GetFlight(model.FlightId);
                if (flight != null)
                {

                    //for (int i = 0; i < booking.NumberPeople; i++)
                    //{
                    //    InsertPassenger(booking.NumberPeople, booking.Id);
                    //}
                    if (booking.IsRoundFlight != true)
                    {
                        var bookingListVM = new BookingListViewModel
                        {
                            NumberSeat = booking.NumberPeople,
                            BookingId = booking.Id,
                            FlightId = model.FlightId,
                            FlightPrice = 0
                        };
                        var bookingList = mapper.Map<BookingList>(bookingListVM);
                        await bookingListRepo.Add(bookingList);
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
                        var bookingGoFlight = mapper.Map<BookingList>(goFlight);
                        var bookingRoundFlight = mapper.Map<BookingList>(roundFlight);
                        booking.BookingLists?.Add(bookingGoFlight);
                        booking.BookingLists?.Add(bookingRoundFlight);
                        await bookingListRepo.Add(bookingGoFlight);
                        await bookingListRepo.Add(bookingRoundFlight);
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
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Some propaties is valid !");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<Response> CancelBooking(Guid bookingId)
        {
            var booking = await bookingRepo.GetById(bookingId);
            if (booking != null)
            {
                booking.Status = "Cancel";
                await bookingRepo.Update(booking);
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
                    checkBooking.FlightPrice = sum;
                    await bookingListRepo.Update(checkBooking);
                    UpdateBooking(checkBooking.BookingId);
                    await unitOfWork.CompletedAsync();
                }
                else
                    checkBooking.FlightPrice += 0;
                return new Response
                {
                    Status = true,
                    Message = "Cancel failed!! Invalid booking",
                    Data = checkBooking
                };
            }
            return new Response
            {
                Status = false,
                Message = "Cancel failed!! Invalid booking"
            };
        }
        public async void UpdateBooking(Guid? bookingId)
        {
            var checkBooking = await bookingRepo.GetById(bookingId);
            if (checkBooking != null)
            {
                checkBooking.TotalPrice = 0;
                var list = bookingListRepo.GetBookingList(bookingId);
                foreach (var item in list)
                {
                    checkBooking.TotalPrice = item.FlightPrice;
                }
                await bookingRepo.Update(checkBooking);
            }
        }

    }
}
