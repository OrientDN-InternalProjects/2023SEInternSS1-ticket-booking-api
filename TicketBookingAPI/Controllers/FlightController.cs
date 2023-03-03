using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TicketBooking.Data;
using TicketBooking.Data.DbContext;
using TicketBooking.Data.DbSeeder;
using TicketBooking.Model.Models;
using TicketBooking.Service.Services.AirportService;
using TicketBooking.Service.Services.FlightService;

namespace TicketBookingAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FlightControllers : ControllerBase
    {
        private IFlightService FlightService { get; }

        public FlightControllers(IFlightService FlightService)
        {
            this.FlightService = FlightService;
        }

        [HttpPost("Add flight")]
        public async Task<ActionResult> AddFlight(FlightRequestModel flightModel)
        {
            if ((flightModel.ArrivalTime.Hour - flightModel.DeparTime.Hour) <= 1 )
            {
                return BadRequest("Wrong in time schedule");
            }
            if (flightModel == null)
            {
                return NotFound();
            }

            await FlightService.InsertAsync(flightModel);
            return Accepted(flightModel.Id);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveFlight(Guid id)
        {
            await FlightService.RemoveAsync(id);
            return Accepted();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateFlight(FlightViewModel flightModel)
        {
            await FlightService.UpdateFlightAsync(flightModel);
            return Accepted();
        }

        [HttpGet]
        public async Task<ActionResult> GetFlight() => Ok(await FlightService.GetFlightAsync());


        [HttpGet("{id}")]
        public async Task<ActionResult> GetFlightbyId(Guid id) => Ok(await FlightService.GetFlightAsync(id));
    }
}