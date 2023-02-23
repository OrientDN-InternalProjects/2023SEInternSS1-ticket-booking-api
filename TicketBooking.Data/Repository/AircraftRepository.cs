using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DbContext;
using TicketBooking.Data.Infrastructure;

namespace TicketBooking.Data.Repository
{
    public interface IAircraftRepository : IRepository<Aircraft, Guid>
    {

    }

    public class AircraftRepository : GenericRepository<Aircraft, Guid>, IAircraftRepository
    {
        public AircraftRepository(TicketBookingDbContext context) : base(context)
        {

        } 
    }
}
