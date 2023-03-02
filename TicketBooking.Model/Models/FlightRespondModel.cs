using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;

namespace TicketBooking.Model.Models
{
    public class FlightRequestModel
    {
        [Required]
        public Guid Id { set; get; }

        [Required]
        public Guid AircraftId { set; get; }

        [Required]
        public bool IsFlightActive { set; get; }

        [Required]
        public int DefaultBaggage { set; get; }

        public decimal BusinessPrice { set; get; }

        public decimal EconomyPrice { set; get; }

        [Required]
        public Guid ScheduleId { set; get; }

        [Required]
        public string DepartAirportCode { get; set; }

        [Required]
        public DateTime DeparTime { get; set; }

        [Required]
        public string ArrivalAirportCode { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }
    }
}
