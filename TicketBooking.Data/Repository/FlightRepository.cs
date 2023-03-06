using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.DbContext;
using TicketBooking.Data.Infrastructure;

namespace TicketBooking.Data.Repository
{
    public interface IFlightRepository : IRepository<Flight>
    {
        public Task<IEnumerable<Flight>> GetFlightByDate(DateTime date);
    }

    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(TicketBookingDbContext context) : base(context)
        {
            
        }
        
        public async Task<IEnumerable<Flight>> GetFlightByDate(DateTime date)
        {
            var convertedDate = date.Date;
            var query = await (from f in _context.Flights
                join fs in _context.FlightSchedules
                    on f.ScheduleId equals fs.Id
                where fs.DepartureTime.Date == date
                select f).ToListAsync();
            

            return  query;
        }
    }
}
