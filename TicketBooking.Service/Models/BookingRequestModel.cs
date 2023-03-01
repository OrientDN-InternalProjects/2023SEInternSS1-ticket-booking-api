using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TicketBooking.Service.Models
{
    public class BookingRequestModel
    {
        [Required]
        public int NumberPeople { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateBooking { get; set; }
        [Required]
        [MaxLength(6)]
        public string? Reference { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public bool IsRoundFlight { get; set; }
        public Guid? ContactId { get; set; }
        public string? UserId { get; set; }
        public Guid FlightId { get; set; }
        public Guid RoundFlightId { get; set; }
    }
}
