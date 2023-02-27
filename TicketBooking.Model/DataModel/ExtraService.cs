using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketBooking.Data.DataModel
{
    [Table("ExtraService")]
    public class ExtraService
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string NameService { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }
        public ICollection<BookingList> BookingLists { get; set; }
        public ExtraService()
        {
            BookingLists = new List<BookingList>();
        }
    }
}
