using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TicketBooking.Data.DataModel;
using TicketBooking.Data;

namespace TicketBooking.Model.Models
{
    public class FlightScheViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public Guid DepartureAirportId { get; set; }
        public Airport AirportDepart { get; set; }
        public Guid ArrivalAirportId { get; set; }
        public Airport AirportArrival { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }
        public ICollection<Flight> Flights { get; set; }
    }
}
