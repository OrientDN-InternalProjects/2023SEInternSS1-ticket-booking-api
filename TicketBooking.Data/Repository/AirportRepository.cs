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
    public interface IAirportRepository : IRepository<Airport>
    {

    }
    public class AirportRepository : GenericRepository<Airport>, IAirportRepository
    {
        public AirportRepository(TicketBookingDbContext context) : base(context)
        {
        }
    }
}
