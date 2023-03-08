using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Model.Models;

namespace TicketBooking.Service.Services.FlightService
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightViewModel>> GetFlightAsync();
        Task<FlightViewModel> GetFlightAsync(Guid id);
        Task<int> UpdateFlightAsync(FlightViewModel flightViewModel);
        Task<Guid> InsertAsync(FlightRequestModel flightRequestModel);
        Task<int> RemoveAsync(Guid id);
        Task<IEnumerable<FlightViewModel>> GetFlightAsync(DateTime date);
        Task<IEnumerable<FlightViewModel>> GetFlightAsync(string departairport, string arrivalairport);
    }
}
