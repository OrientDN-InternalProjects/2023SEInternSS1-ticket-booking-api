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

            flightModel.ArrivalAirportCode = flightvalidate.AirportNameProcess(flightModel.ArrivalAirportCode);
            flightModel.DepartAirportCode = flightvalidate.AirportNameProcess(flightModel.DepartAirportCode);
            
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
        public async Task<ActionResult> UpdateFlight(FlightUpdateModel flightUpdateModel)
        {
            var result = await flightservice.UpdateFlightAsync(flightUpdateModel);
            if (result == 0)
            {
                return BadRequest("Cannot update flight. Flight already ended or not exist");
            }
            return Accepted();
        }

        [HttpGet]
        public async Task<ActionResult> GetFlight() => Ok(await flightservice.GetFlightAsync());

        [HttpGet("GetflightByID")]
        public async Task<ActionResult> GetFlightById(Guid id)
        {
            if ((await flightservice.GetFlightAsync(id)) == null)
            {
                return BadRequest("No flight matches with the ID");
            }
            
            return Ok(await flightservice.GetFlightAsync(id));
        }
        
        [HttpGet("GetflightByRequest")]
        public async Task<ActionResult> GetFlightByRequest([FromQuery] FlightRequest flightrequest)
        {
            if ((await flightservice.GetFlightAsync(flightrequest)) == null)
            {
                return BadRequest("No flight matches with the ID");
            }
            
            return Ok(await flightservice.GetFlightAsync(flightrequest));
        }

        // This controller is for logic demo purpose.
        // It will be removed after being integrated into booking service
        [HttpPut("UpdateRemainSeat")]
        public async Task<ActionResult> UpdateRemainingSeat(Guid id, SeatClassType type, int number)
        {
            if((await flightservice.UpdateFlightSeat(id, type, number)))
            {
                return Ok("Update remaining seat successfully");
            }
            else
            {
                return BadRequest("Update remaining seat failed");
            }
        }
        
}
}