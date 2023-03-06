using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;

namespace TicketBooking.Model
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }
        public long Amount { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }

        public long PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }

    }
}
