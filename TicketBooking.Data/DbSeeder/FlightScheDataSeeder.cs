using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DbContext;

namespace TicketBooking.Data.DbSeeder
{
    public class FlightScheDataSeeder : IFlightScheduleDataSeeder
    {
        private readonly ILogger<AirportDataSeeder> logger;
        private readonly TicketBookingDbContext dbContext;

        public FlightScheDataSeeder(ILogger<AirportDataSeeder> _logger, TicketBookingDbContext context)
        {
            logger = _logger;
            dbContext = context;
        }

        public void InitDataBase()
        {
            if (!dbContext.FlightSchedules.Any())
            {
                var flightSche = new List<FlightSchedule>()
                {
                    new FlightSchedule()
                    {
                        Id = Guid.Parse("c654b73f-dc45-4725-b44e-d9f5f0e697d3"),
                        // DepartureAirportId = Guid.Parse("8e204406-9d1b-45da-a643-2ba07ff9fca5"),
                        // AirportDepart = new Airport()
                        // {
                        //     Id = Guid.Parse("8e204406-9d1b-45da-a643-2ba07ff9fca5"),
                        //     Name = "Tan Son Nhat",
                        //     City = "Ho Chi Minh",
                        //     Country = "Viet Nam"
                        // },
                        // ArrivalAirportId = Guid.Parse("7fc1f3c4-1244-4f70-b663-cf0e322a523e"),
                        // AirportArrival = new Airport()
                        // {
                        //     Id = Guid.Parse("7fc1f3c4-1244-4f70-b663-cf0e322a523e"),
                        //     Name = "Noi Bai",
                        //     City = "Ha Noi",
                        //     Country = "Viet Nam"
                        // },
                        DepartureTime = new DateTime(2023, 3, 2),
                        ArrivalTime = new DateTime(2016, 3, 3)
                    }
                };

                dbContext.FlightSchedules.AddRange(flightSche);
            }
        }
    }
}
