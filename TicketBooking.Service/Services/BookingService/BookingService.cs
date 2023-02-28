using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Model.Models;

namespace TicketBooking.Service.Services.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository bookingRepo;
        private readonly IBookingListRepository bookingListRepo;
        private readonly IBookingSeatRepository bookingSeatRepo;
        private readonly IPassengerRepository passengerRepo;
        private readonly ITicketRepository ticketRepo;
        private readonly IContactDetailRepository contactRepo;
        private IUnitOfWork unitOfWork;
        private readonly IFlightRepository flightRepo;
        private readonly IMapper mapper;

        public BookingService(IBookingRepository booking
            , IUnitOfWork unitOfWork
            , IMapper mapper
            , ITicketRepository tickeetRepo
            , IPassengerRepository passenger
            , IBookingSeatRepository bookingSeat
            , IBookingListRepository bookingList
            , IContactDetailRepository contactRepo
            , IFlightRepository flightRepo)
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

        public async Task<Response> RequestBooking(BookingViewModel model, Guid flightId, Guid? roundFlight)
        {
            var booking = mapper.Map<Booking>(model);
            var flight = await flightRepo.GetFlight(flightId);
            if (flight != null)
            {
                await bookingRepo.AddBooking(booking);
                for (int i = 0; i < booking.NumberPeople; i++)
                {
                    InsertPassenger(booking.NumberPeople, booking.Id);
                }
                if (booking.IsRoundFlight != true)
                {
                    await bookingListRepo.AddListBooking(new BookingList { NumberSeat = booking.NumberPeople, BookingId = booking.Id, FlightId = flightId });
                }
                else if(roundFlight != null)
                {
                    await bookingListRepo.AddListBooking(new BookingList { NumberSeat = booking.NumberPeople, BookingId = booking.Id, FlightId = flightId });
                    await bookingListRepo.AddListBooking(new BookingList { NumberSeat = booking.NumberPeople, BookingId = booking.Id, FlightId = roundFlight }); 
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
    }
}
