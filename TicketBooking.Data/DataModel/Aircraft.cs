using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBooking.Data
{
    [Table("Aircraft")]
    public class Aircraft
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(6)]
        public string Model { get; set; } = null!;
        [Required]
        [MaxLength(6)]
        public string Manufacture { get; set; }
        public ICollection<Flight> Flights { get; set; }
        public Aircraft()
        {
            Flights = new List<Flight>();
        }
    }
}
