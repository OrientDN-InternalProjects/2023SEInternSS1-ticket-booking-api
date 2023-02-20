using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBooking.Data
{
    [Table("SeatClass")]
    public class SeatClass
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string SeatName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public ICollection<Seat> Seats { get; set; }
        public SeatClass()
        {
            Seats = new List<Seat>();
        }
    }
}
