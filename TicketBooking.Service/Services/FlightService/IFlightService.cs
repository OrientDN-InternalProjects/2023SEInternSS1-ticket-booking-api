﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data;
using TicketBooking.Common.AppExceptions;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Model.Models;

namespace TicketBooking.Service.Services.FlightService
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightViewModel>> GetFlightAsync();
        Task<IEnumerable<FlightViewModel>> GetFlightAsync(Guid id);
        Task<int> UpdateFlightAsync(FlightUpdateModel flightUpdateModel);
        Task<Guid> InsertAsync(FlightRequestModel flightRequestModel);
        Task<int> RemoveAsync(Guid id);
        Task<bool> UpdateFlightSeat(Guid flightId, SeatClassType type, int number);
        Task<IEnumerable<FlightViewModel>> GetFlightAsync(FlightRequest flightModel);
    }
}
