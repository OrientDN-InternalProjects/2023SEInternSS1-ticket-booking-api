using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Model.Models;

namespace TicketBooking.Service.Services.BookingService
{
    public interface IBookingService
    {
        Task<Response> RequestBooking(BookingViewModel model, Guid flightId, Guid? roundFlight);
    }
}
