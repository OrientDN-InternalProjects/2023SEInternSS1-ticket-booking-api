using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Service.Models;

namespace TicketBooking.Service.Services.BookingService
{
    public interface IBookingService
    {
        Task<Response> RequestBooking(BookingRequestModel model);
        Task<Response> CancelBooking(Guid bookingId);
        Task<Response> AddBookingService(List<Guid> services, Guid bookingList);
    }
}
