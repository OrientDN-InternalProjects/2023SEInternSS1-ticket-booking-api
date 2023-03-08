using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
using TicketBooking.Common.AppExceptions;

namespace TicketBookingAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FlightControllers : ControllerBase
    {
        private IFlightService flightservice { get; }
        private IFlightValidation flightvalidate { get; }

        public FlightControllers(IFlightService flightservice,
            IFlightValidation flightvalidate)
        {
            this.flightservice = flightservice;
            this.flightvalidate = flightvalidate;
        }


        [HttpPost("add_flight")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddFlight(FlightRequestModel flightModel)
        {
            if (flightModel == null)
            {
                return BadRequest("No model to enter");
            }

            if (!flightvalidate.FlightDateValid(flightModel.DepartTime, flightModel.ArrivalTime))
            {
                return BadRequest("Wrong in time schedule");
            }

            if (!flightvalidate.AirportNameValid(flightModel.DepartAirportCode)
                && !flightvalidate.AirportNameValid(flightModel.ArrivalAirportCode))
            {
                return BadRequest("Wrong airport name");
            }

            return Accepted(await flightservice.InsertAsync(flightModel));
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveFlight(Guid id)
        {
            await flightservice.RemoveAsync(id);
            return Accepted();
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateFlight(FlightViewModel flightModel)
        {
            await flightservice.UpdateFlightAsync(flightModel);
            return Accepted();
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetFlight() => Ok(await flightservice.GetFlightAsync());
        
        [HttpGet("GetFlightbyDate")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetFlightByDate(DateTime dateRequest)
        {
            if (!(await flightservice.GetFlightAsync(dateRequest)).Any())
            {
                return BadRequest("There is no flight on that day");
            }

            return Ok(await flightservice.GetFlightAsync(dateRequest));
        }


        [HttpGet("GetFlightbyAirport")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> GetFlightByAirport(string departairport, string arrivalairport)
        {
            if (!(await flightservice.GetFlightAsync(departairport, arrivalairport)).Any())
            {
                return BadRequest("No flight is hosted");
            }

            return Ok(await flightservice.GetFlightAsync(departairport, arrivalairport));
        }

        [HttpGet("GetflightByID")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetFlightById(Guid id)
        {
            if ((await flightservice.GetFlightAsync(id)) == null)
            {
                return BadRequest("No flight matches with the ID");
            }
            
            return Ok(await flightservice.GetFlightAsync(id));
        }
}
}