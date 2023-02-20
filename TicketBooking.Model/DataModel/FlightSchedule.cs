using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBooking.Data
{
    [Table("FlightSchedule")]
    public class FlightSchedule
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DepartureAirportId { get; set; }
        public Airport AirportDepart { get; set; }
        [Required]
        public int ArrivalAirportId { get; set; }
        public Airport AirportArrival { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }
        public ICollection<Flight> Flights { get; set; }
        public FlightSchedule()
        {
            Flights = new List<Flight>();
        }
    }
}
