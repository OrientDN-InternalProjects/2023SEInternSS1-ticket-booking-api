using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TicketBooking.Data.DataModel;

namespace TicketBooking.Model.Models
{
    public class FlightViewModel
    { 
        [Required]
        public Guid Id { set; get; }
        
        public string? AircraftModel { set; get; }

        public string? AircraftManufacture { get; set; }
        
        [Required]
        public bool IsFlightActive { set; get; }

        [Required]
        public int DefaultBaggage { set; get; }

        public decimal BusinessPrice { set; get; }

        public decimal EconomyPrice { set; get; }

        public DateTime DepartTime { set; get; }
    }
}
