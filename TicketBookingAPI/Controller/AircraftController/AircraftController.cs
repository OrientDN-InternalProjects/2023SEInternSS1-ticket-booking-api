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
        public async Task<ActionResult> AddAircraft(AircraftDTO aircraftDTO)
        {
            if (aircraftDTO == null)
            {
                return NotFound();
            }

            try
            {
                _service.InsertAsync(aircraftDTO);
                _service.CompleteAsync();
                return Ok("Added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveAircraft(Guid id)
        {
            await _service.RemoveAsync(id);
            await _service.CompleteAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAirCraft(AircraftDTO aircraftDTO)
        {
            await _service.UpdateAircraftAsync(aircraftDTO);
            await _service.CompleteAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetAircraft()
        {
            await _service.GetAircraftAsync();
            await _service.CompleteAsync();
            return Ok();
        }
    }
}