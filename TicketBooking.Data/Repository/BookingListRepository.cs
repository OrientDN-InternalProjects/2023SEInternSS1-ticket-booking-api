using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.DbContext;
using TicketBooking.Data.Infrastructure;

namespace TicketBooking.Data.Repository
{
    public interface IBookingListRepository
    {
        Task<bool> AddListBooking(BookingList list);
    }
    public class BookingListRepository : GenericRepository<BookingList>, IBookingListRepository
    {
        public BookingListRepository(TicketBookingDbContext context) : base(context)
        {
        }
        public async Task<bool> AddListBooking(BookingList list)
        {
            return await Add(list);
        }
    }
}
