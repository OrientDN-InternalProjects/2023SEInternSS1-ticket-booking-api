using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.Infrastructure;

namespace TicketBooking.Data.Repository
{
    public interface IAircraftRepository : IRepository<Aircraft, Guid>
    {

    }

    public class AircraftRepository : GenericRepository<Aircraft, Guid>, IAircraftRepository
    {
        // To keep the folder from being skipped by gitignore
    }
}
