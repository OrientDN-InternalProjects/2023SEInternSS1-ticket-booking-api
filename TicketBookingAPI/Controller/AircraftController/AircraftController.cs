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
        private IAircraftSerivce Service { get; }
        public AircraftController(IAircraftSerivce service)
        {
            Service = service;
        }

        [HttpPost]
        public async Task<ActionResult> AddAircraft(AircraftViewModel? aircraftModel)
        {
            if (aircraftModel == null)
            {
                return NotFound();
            }
            else
            {
                await Service.InsertAsync(aircraftModel);
                return Accepted("{id} Added", aircraftModel.Id);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveAircraft(Guid id)
        {
            Service.RemoveAsync(id);
            return Accepted();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAircraft(AircraftViewModel aircraftDto)
        {
            await Service.UpdateAircraftAsync(aircraftDto);
            return Accepted();
        }

        [HttpGet]
        public async Task<ActionResult> GetAircraft() => Ok(await Service.GetAircraftAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAircraftbyId(Guid id) => Ok(await Service.GetAircraftAsync(id));
    }
}