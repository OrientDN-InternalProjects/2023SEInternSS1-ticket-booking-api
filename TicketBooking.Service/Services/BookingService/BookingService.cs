using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using TicketBooking.Common.Constant;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Model.DataModel;
using TicketBooking.Service.Models;
using TicketBooking.Service.Services.SendMailService;

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
        private readonly ISendMailService sendMailService;
        private readonly IFlightScheduleRepository scheduleRepo;
        private readonly IAirportRepository airportRepo;
        private readonly IAircraftRepository aircraftRepo;

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
            , IExtraServiceRepository extra
            , UserManager<ApplicationUser> userManager
            , ISendMailService sendMailService
            , IFlightScheduleRepository scheduleRepo,
                IAirportRepository airportRepo,
                IAircraftRepository aircraftRepo)
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
            this.userManager = userManager;
            this.sendMailService = sendMailService;
            this.scheduleRepo = scheduleRepo;
            this.airportRepo = airportRepo;
            this.aircraftRepo = aircraftRepo;
        }

        public async Task<string> RequestBooking(BookingRequestModel model)
        {
            var flight = await flightRepo.GetFlight(model.FlightId);
            var backFlight = await flightRepo.GetFlight(model.RoundFlightId.Value);
            var contact = await contactRepo.GetById(model.ContactId.Value);
            var seatType = string.Empty;
            if (flight == null || contact == null)
            {
                return "Please check your data input ";

            }

            var booking = new Booking
            {
                NumberPeople = model.Passes.Count,
                DateBooking = DateTime.Now,
                Reference = $"BK{contact.Id.ToString().Substring(0, 4)}",
                TotalPrice = 0,
                IsPaid = false,
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
            if (model.Passes.Count > 0)
            {
                booking = await AddPassenger(booking, model.Passes);
                bookingRepo.Update(booking);
            }
            else
            {
                return "Invalid Passenger";
            }
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
            }
            else if (backFlight != null)
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
                    seatType = "Business";
                    goFlight.FlightPrice = flight.BusinessPrice * goFlight.NumberSeat;
                    roundFlight.FlightPrice = backFlight.BusinessPrice * roundFlight.NumberSeat;
                }
                else
                {
                    seatType = "Economy";
                    goFlight.FlightPrice = flight.EconomyPrice * goFlight.NumberSeat;
                    roundFlight.FlightPrice = backFlight.EconomyPrice * roundFlight.NumberSeat;
                }
                await bookingListRepo.Add(goFlight);
                await bookingListRepo.Add(roundFlight);
                await unitOfWork.CompletedAsync();
                goFlight = await ExtraService(goFlight, model.Services);
                roundFlight = await ExtraService(roundFlight, model.Services);
                bookingListRepo.Update(goFlight);
                bookingListRepo.Update(roundFlight);
                booking.BookingLists?.Add(goFlight);
                booking.BookingLists?.Add(roundFlight);

                booking.TotalPrice = goFlight.FlightPrice + roundFlight.FlightPrice;

            }
            else
            {
                return "Booking failed, Invalid round trip flight";
            }
            booking = await ExportTicket(booking, seatType);
            bookingRepo.Update(booking);
            await unitOfWork.CompletedAsync();
            return booking.Id.ToString();

        }

        public async Task<Booking> AddPassenger(Booking booking, List<PassengerViewModel> passes)
        {
            foreach (var passenger in passes)
            {
                var newPassenger = mapper.Map<Passenger>(passenger);
                newPassenger.BookingId = booking.Id;
                booking.Passengers.Add(newPassenger);
                await passengerRepo.AddPassenger(newPassenger);
            }
            await unitOfWork.CompletedAsync();
            return booking;
        }



        public async Task<BookingList> ExtraService(BookingList bookingList, List<Guid> extraServices)
        {
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

        public async Task<Booking> ExportTicket(Booking booking, string seatType)
        {
            var bookingList = bookingListRepo.Find(x => x.BookingId == booking.Id);
            var flights = new List<Flight>();
            var tickets = new List<Ticket>();
            var aircrafts = new List<Aircraft>();
            foreach (var item in bookingList)
            {
                var flight = await flightRepo.GetById((Guid)item.FlightId);
                flights.Add(flight);

            }
            var schedules = await AddScheduleAsync(flights);
            var airports = await AddAirport(schedules);

            foreach(var flight in flights)
            {
                var aircraft = await aircraftRepo.GetById(flight.AircraftId);
                aircrafts.Add(aircraft);
            }
            
            foreach (var passenger in booking.Passengers)
            {
                if (flights.Count > 1)
                {
                    var ticketGo = new Ticket
                    {
                        PassengerName = $"{passenger.FirstName} {passenger.LastName}",
                        BookingId = booking.Id,
                        LocationFrom = airports["DepartGo"].City,
                        LocationTo = airports["ArivalGo"].City,
                        SeatClass = seatType,
                        DepartureTime = schedules[0].DepartureTime,
                        AirlineName = "VietNam Airline",
                        AircraftModel = aircrafts[0].Manufacture,
                        BookingCode = booking.Reference,
                        PassengerId = passenger.Id,
                    };
                    var ticketRound = new Ticket
                    {
                        PassengerName = $"{passenger.FirstName} {passenger.LastName}",
                        BookingId = booking.Id,
                        LocationFrom = airports["DepartBack"].City,
                        LocationTo = airports["ArivalBack"].City,
                        SeatClass = seatType,
                        DepartureTime = schedules[1].DepartureTime,
                        AirlineName = "VietNam Airline",
                        AircraftModel = aircrafts[1].Manufacture,
                        BookingCode = booking.Reference,
                        PassengerId = passenger.Id,
                    };
                    await ticketRepo.AddTicket(ticketRound);
                    await ticketRepo.AddTicket(ticketGo);
                    await unitOfWork.CompletedAsync();
                    tickets.Add(ticketRound);
                    tickets.Add(ticketGo);
                    passenger.Tickets.Add(ticketGo);
                    passenger.Tickets.Add(ticketRound);
                    passengerRepo.Update(passenger);
                    booking.Tickets.Add(ticketGo);
                    booking.Tickets.Add(ticketRound);
                    
                }
                else
                {
                    var ticketGo = new Ticket
                    {
                        PassengerName = $"{passenger.FirstName} {passenger.LastName}",
                        BookingId = booking.Id,
                        LocationFrom = airports["DepartGo"].City,
                        LocationTo = airports["ArivalGo"].City,
                        SeatClass = seatType,
                        DepartureTime = schedules[0].DepartureTime,
                        AirlineName = "VietNam Airline",
                        AircraftModel = aircrafts[0].Manufacture,
                        BookingCode = booking.Reference,
                        PassengerId = passenger.Id,
                    };
                    await ticketRepo.AddTicket(ticketGo);
                    await unitOfWork.CompletedAsync();
                    passenger.Tickets.Add(ticketGo);
                    tickets.Add(ticketGo);
                    passengerRepo.Update(passenger);
                    booking.Tickets.Add(ticketGo);
                }
            }
            await unitOfWork.CompletedAsync();
            return booking;
        }

        private async Task<List<FlightSchedule>> AddScheduleAsync(List<Flight> flights)
        {
            var schedules = new List<FlightSchedule>();
            foreach (var flight in flights)
            {
                var schedule = await scheduleRepo.GetById(flight.ScheduleId);
                schedules.Add(schedule);
            }
            return schedules;
        }

        private async Task<IDictionary<string, Airport>> AddAirport(List<FlightSchedule> schedules)
        {
            var airports = new Dictionary<string, Airport>();
            if (schedules.Count > 1)
            {
                var departGo = await airportRepo.GetById(schedules[0].DepartureAirportId);
                var arivalGo = await airportRepo.GetById(schedules[0].ArrivalAirportId);
                var departBack = await airportRepo.GetById(schedules[1].DepartureAirportId);
                var arivalBack = await airportRepo.GetById(schedules[1].ArrivalAirportId);
                airports.Add("DepartGo", departGo);
                airports.Add("ArivalGo", arivalGo);
                airports.Add("DepartBack", departBack);
                airports.Add("ArivalBack", arivalBack);

            }
            else
            {
                var departGo = await airportRepo.GetById(schedules[0].DepartureAirportId);
                var arivalGo = await airportRepo.GetById(schedules[0].ArrivalAirportId);
                airports.Add("DepartGo", departGo);
                airports.Add("ArivalGo", arivalGo);
            }
            return airports;
        }

        public Task<Response> ConfirmMailBooking(Guid bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
