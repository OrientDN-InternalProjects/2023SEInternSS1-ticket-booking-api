using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Model.Models;

namespace TicketBooking.Service.Services.AirportService
{
    public interface IAirportService
    {
        Task<IEnumerable<AirportViewModel>> GetAirportAsync();
        Task<AirportViewModel> GetAirportAsync(Guid id);
        Task<AirportViewModel> GetAirportAsync(string code);
        Task<int> UpdateAirportAsync(AirportViewModel airportViewModel);
        Task<int> InsertAsync(AirportViewModel airportViewModel);
        Task<int> RemoveAsync(Guid id);
    }
}
