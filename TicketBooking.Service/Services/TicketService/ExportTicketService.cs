using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Service.Models;

namespace TicketBooking.Service.Services.TicketService
{
    public class ExportTicketService : IExportTicket
    {
        private IUnitOfWork unitOfWork;
        private readonly TicketRepository ticketRepo;
        private readonly FlightRepository flightRepo;
        private readonly PassengerRepository passengerRepo;
        private readonly BookingRepository bookingRepo;
        public Task<List<TicketViewModel>> ExportTicket(Guid bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
