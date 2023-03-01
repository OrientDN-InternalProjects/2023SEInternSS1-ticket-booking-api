using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;

namespace TicketBooking.Service.Models
{
    public class BookingListViewModel
    {
        [Required]
        public Guid BookingId { get; set; }
        [Required]
        public Guid? FlightId { get; set; }
        [Required]
        public int NumberSeat { get; set; }
        [Required]
        public decimal FlightPrice { get; set; }
    }
}
