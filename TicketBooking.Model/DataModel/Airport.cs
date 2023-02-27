using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBooking.Data
{
    [Table("Airport")]
    public class Airport
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        public string Country { get; set; }
        public ICollection<FlightSchedule> DepartureAirports { get; set; }
        public ICollection<FlightSchedule> ArrivalAirports { get; set; }
        public Airport()
        {
            DepartureAirports = new List<FlightSchedule>();
            ArrivalAirports = new List<FlightSchedule>();
        }
    }
}
