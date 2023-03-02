using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Data.DataModel
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Number People")]
        public int NumberPeople { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Booking")]
        public DateTime DateBooking { get; set; }

        [Required]
        [MaxLength(6)]
        public string Reference { get; set; }

        [Required]
        [Display(Name = "Total Price")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        public bool IsPaid { get; set; }
        public string? Status { get; set; }
        [Required]
        public bool IsRoundFlight { get; set; }

        public Guid? ContactId { get; set; }
        public ContactDetail? ContactDetail { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<BookingList>? BookingLists { get; set; }
        public ICollection<Passenger>? Passengers { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
        public Booking()
        {
            Tickets= new List<Ticket>();
            Passengers = new List<Passenger>();
            BookingLists = new List<BookingList>(); 
        }
    }
}
