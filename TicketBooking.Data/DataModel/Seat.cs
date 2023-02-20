using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBooking.Data
{
    [Table("Seat")]
    public class Seat
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string SeatNumber { get; set; }
        [Required]
        public Guid SeatClassId { get; set; }
        public SeatClass SeatClass { get; set; }
        [Required]
        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }
        //One to one relationship with ListSeat
        public ListSeat ListSeat { get; set; }
    }
}
