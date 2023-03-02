using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Data.DataModel
{
    [Table("Passenger")]
    public class Passenger
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required, MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string Nation { get; set; }

        [Required, MaxLength(20)]
        public string IdentityCard { get; set; }

        [Required, MaxLength(50)]
        [Display(Name ="Provide Nation")]
        [Column(TypeName = "varchar")]
        public string ProvideNa { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Expire Nation")]
        public DateTime ExpDate { get; set; }

        public Guid? BookingId { get; set; }

        public Booking? Booking { get; set; }

        public Ticket? Ticket { get; set; }
    }
}
