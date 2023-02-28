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
using TicketBooking.Service.Services.AircraftService;
using TicketBooking.Data.DbSeeder;

namespace TicketBookingAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private IAircraftSerivce Aircraftservice { get; }
        private IDataSeeder dataSeeder { get; }
        //private IDataSeeder dataSeeder { get; }

        public AircraftController(IAircraftSerivce service, IDataSeeder dataSeeder)
        {
            Aircraftservice = service;
            this.dataSeeder = dataSeeder;
            //this.dataSeeder = dataSeeder;
        }

        [HttpPost]
        public async Task<ActionResult> AddAircraft(AircraftViewModel aircraftModel)
        {
            if (aircraftModel == null)
            {
                return NotFound();
            }

            if (aircraftModel.Model?.Length > 6 || aircraftModel.Manufacture?.Length > 10)
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
        // public async Task<ActionResult> GetAircraft() => Ok(await Aircraftservice.GetAircraftAsync());        
        public async Task<ActionResult> GetAircraft()
        {
            dataSeeder.InitDataBase();
            return Ok(await Aircraftservice.GetAircraftAsync());
        }
        

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAircraftbyId(Guid id) => Ok(await Aircraftservice.GetAircraftAsync(id));
    }
}