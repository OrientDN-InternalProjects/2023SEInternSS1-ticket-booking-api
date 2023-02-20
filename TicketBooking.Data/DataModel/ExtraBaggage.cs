using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Data.DataModel
{
    [Table("ExtraBaggage")]
    public class ExtraBaggage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public byte Weight { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ExtraBaggage()
        {
            Bookings = new List<Booking>();
        }
    }
}
