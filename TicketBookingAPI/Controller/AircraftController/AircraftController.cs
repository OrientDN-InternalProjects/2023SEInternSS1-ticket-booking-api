using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TicketBooking.Data;
using TicketBooking.Service.AircraftService;
using TicketBooking.Data.DbContext;
using TicketBooking.Service.Model;

namespace TicketBookingAPI.Controller.AircraftController
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private IAircraftSerivce Aircraftservice { get; }
        public AircraftController(IAircraftSerivce service)
        {
            Aircraftservice = service;
        }

        [HttpPost]
        public async Task<ActionResult> AddAircraft(AircraftViewModel aircraftModel)
        {
            if (aircraftModel == null)
            {
                return NotFound();
            }

            await Aircraftservice.InsertAsync(aircraftModel);
            return Accepted(aircraftModel.Id);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveAircraft(Guid id)
        {
            await Aircraftservice.RemoveAsync(id);
            return Accepted();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAircraft(AircraftViewModel aircraftModel)
        {
            await Aircraftservice.UpdateAircraftAsync(aircraftModel);
            return Accepted();
        }

        [HttpGet]
        public async Task<ActionResult> GetAircraft() => Ok(await Aircraftservice.GetAircraftAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAircraftbyId(string id) => Ok(await Aircraftservice.GetAircraftAsync(id));
    }
}