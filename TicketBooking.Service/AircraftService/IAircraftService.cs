using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Service.Model;

namespace TicketBooking.Service.AircraftService
{
    public interface IAircraftSerivce
    {
        Task<IEnumerable<AircraftViewModel>> GetAircraftAsync();
        Task<AircraftViewModel> GetAircraftAsync(string id);
        Task<int> UpdateAircraftAsync(AircraftViewModel aircraftDto);
        Task<int> InsertAsync(AircraftViewModel aircraftDto);
        Task<int> RemoveAsync(Guid id);
    }
}
