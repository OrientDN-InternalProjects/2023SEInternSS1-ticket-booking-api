using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Data.DataModel
{
    [Table("Ticket")]
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [Display(Name ="Passenger Name")]
        [Column(TypeName = "varchar")]
        public string PassengerName { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name ="Location From")]
        [Column(TypeName = "varchar")]
        public string LocationFrom { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Location To")]
        [Column(TypeName = "varchar")]
        public string LocationTo { get; set; }
        [Required]
        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string SeatClass { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string AirlineName { get; set; }
        [Required]
        [MaxLength(6)]
        public string AircraftModel { get; set; }
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
        public Guid PassengerId { get; set; }
        public Passenger Passenger { get; set; }
    }
}
