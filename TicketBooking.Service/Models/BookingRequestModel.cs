using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TicketBooking.Data.DataModel;

namespace TicketBooking.Service.Models
{
    public class BookingRequestModel
    {
        [Required]
        public int NumberPeople { get; set; }
        [Required]
        public DateTime DateBooking { get; set; }
        [Required]
        public string Reference { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        public Guid? ContactId { get; set; }
        [Required]
        public bool IsRoundFlight { get; set; }
        public bool IsBusiness { get; set; }
        public Guid? UserId { get; set; }
        public Guid FlightId { get; set; }
        public Guid? RoundFlightId { get; set; }
        public List<ListRequest>? Requests { get; set; }
        public List<Guid> Services { get; set; }
    }
}
