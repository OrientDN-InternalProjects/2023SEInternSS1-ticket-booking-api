using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketBooking.Data.DataModel;
using TicketBooking.Model.DataModel;

namespace TicketBooking.Data.DataModel
{
    [Table("BookingList")]
    public class BookingList
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
        [Required]
        public Guid? FlightId { get; set; }
        public Flight? Flight { get; set; }
        [Required]
        public int NumberSeat { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal FlightPrice { get; set; }
        public ICollection<BookingSeat>? ListSeats { get; set; }
        public ICollection<BookingExtraService>? BookingServices { get; set; }
        public BookingList()
        {
            ListSeats = new List<BookingSeat>();
            BookingServices = new List<BookingExtraService>();
        }
    }
}
