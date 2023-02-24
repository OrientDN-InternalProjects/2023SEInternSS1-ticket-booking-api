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
        private IAircraftSerivce _service { get; }
        public AircraftController(IAircraftSerivce service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> AddAircraft(AircraftViewModel aircraftModel)
        {
            if (aircraftModel == null)
            {
                return NotFound();
            }
            else
            {
                await _service.InsertAsync(aircraftModel);
                return Accepted(aircraftModel.Id);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveAircraft(Guid id)
        {
            _service.RemoveAsync(id);
            return Accepted();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAircraft(AircraftViewModel aircraftModel)
        {
            await _service.UpdateAircraftAsync(aircraftModel);
            return Accepted();
        }

        [HttpGet]
        public async Task<ActionResult> GetAircraft() => Ok(await _service.GetAircraftAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAircraftbyId(Guid id) => Ok(await _service.GetAircraftAsync(id));
    }
}