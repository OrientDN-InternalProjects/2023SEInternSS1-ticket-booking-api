using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TicketBooking.Data.DbContext;

namespace TicketBooking.Data.DbSeeder
{
    public class DataSeeder : IDataSeeder, IAircraftDataSeeder
    {

        private readonly ILogger<DataSeeder> logger;
        private readonly TicketBookingDbContext dbContext;
        private readonly IAircraftDataSeeder aircraftDataSeeder;
        private readonly IAirportDataSeeder airportDataSeeder;
        private readonly ISeatClassDataSeeder seatClassDataSeeder;
        //private readonly IFlightScheduleDataSeeder flightScheDataSeeder;
        public DataSeeder(ILogger<DataSeeder> logger,
                          TicketBookingDbContext dbContext,
                          IAircraftDataSeeder aircraftDataSeeder,
                          IAirportDataSeeder airportDataSeeder,
                          //IFlightScheduleDataSeeder flightScheDataSeeder
                          )
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.aircraftDataSeeder = aircraftDataSeeder;
            this.airportDataSeeder = airportDataSeeder;
            //this.flightScheDataSeeder = flightScheDataSeeder;
        }
        public async Task InitDataBase()
        {
             // seeding data for aircraft
             await aircraftDataSeeder.InitDataBase();
             
             // seeding data for airport
             await airportDataSeeder.InitDataBase();
             
             // Seeding data for seat class
             await seatClassDataSeeder.InitDataBase();

             //await flightScheDataSeeder.InitDataBase();

             dbContext.SaveChanges();

        }
    }
}
