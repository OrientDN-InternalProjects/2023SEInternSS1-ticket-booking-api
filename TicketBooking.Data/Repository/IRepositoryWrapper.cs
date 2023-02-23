using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Data.Repository
{
    public interface IRepositoryWrapper
    {
        IAircraftRepository _aircrafts { get; }
    }
}
