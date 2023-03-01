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
            var booking = mapper.Map<Booking>(model);
            var flight = await flightRepo.GetFlight(model.FlightId);
            if (flight != null)
            {
                await bookingRepo.AddBooking(booking);
                //for (int i = 0; i < booking.NumberPeople; i++)
                //{
                //    InsertPassenger(booking.NumberPeople, booking.Id);
                //}
                if (booking.IsRoundFlight != true)
                {
                    await bookingListRepo.Add(new BookingList { Id=new Guid(), NumberSeat = booking.NumberPeople, BookingId = booking.Id, FlightId = model.FlightId });
                    await unitOfWork.CompletedAsync();
                }
                else if (model.RoundFlightId != null)
                {
                    var goFlight = new BookingList { 
                        Id = Guid.NewGuid(), 
                        NumberSeat = booking.NumberPeople, 
                        BookingId = booking.Id, 
                        FlightId = model.FlightId 
                    };
                    var roundFlight = new BookingList
                    {
                        Id = Guid.NewGuid(),
                        NumberSeat = booking.NumberPeople,
                        BookingId = booking.Id,
                        FlightId = model.RoundFlightId
                    };
                    await bookingListRepo.Add(goFlight);
                    await bookingListRepo.Add(roundFlight);
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
                booking.Status = "Cancel";
                await bookingRepo.Update(booking);
                await unitOfWork.CompletedAsync();
                return new Response
                {
                    Status = true,
                    Message = "Cancel successfully"
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
        public async void UpdateBooking (Guid bookingId)
        {
            var checkBooking = await bookingRepo.GetById(bookingId);
            if (checkBooking != null)
            {
                checkBooking.TotalPrice = 0;
                var list = bookingListRepo.GetBookingList(bookingId);
                foreach(var item in list)
                {
                    checkBooking.TotalPrice = item.FlightPrice;
                }
                await bookingRepo.Update(checkBooking);
                await unitOfWork.CompletedAsync();
            }
        }

    }
}
