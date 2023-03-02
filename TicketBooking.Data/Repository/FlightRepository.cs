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
    public interface IFlightRepository : IRepository<Flight>
    {

    }

    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(TicketBookingDbContext context) : base(context)
        {

        }
    }
}
