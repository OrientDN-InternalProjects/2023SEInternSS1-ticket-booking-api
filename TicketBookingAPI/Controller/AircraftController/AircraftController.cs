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

namespace TicketBookingAPI.Controller.AircraftController
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly AircraftService _service;

        public AircraftController(AircraftService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> AircraftRequestModel(AircraftDTO? aircraftDto)
        {
            if (aircraftDto == null)
            {
                return NotFound();
            }
            else
            {
                await _service.InsertAsync(aircraftDto);
                await _service.CompleteAsync();
                return Accepted("Added");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveAircraft(Guid id)
        {
            await _service.RemoveAsync(id);
            await _service.CompleteAsync();
            return Accepted();
        }

        [HttpPut]
        public async Task<ActionResult> AircraftModelUpdate(AircraftDTO aircraftDto)
        {
            await _service.UpdateAircraftAsync(aircraftDto);
            await _service.CompleteAsync();
            return Accepted();
        }

        [HttpGet]
        public async Task<ActionResult> GetAircraft() => Accepted(await _service.GetAircraftAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAircraftbyId(Guid id) => Accepted(await _service.GetAircraftAsync(id));
    }
}