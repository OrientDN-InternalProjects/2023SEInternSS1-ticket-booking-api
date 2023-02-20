using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Data.DataModel
{
    [Table("Report")]
    public class Report
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal BookingPrice { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateBooking { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
