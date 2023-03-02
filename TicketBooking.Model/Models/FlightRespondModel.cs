using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;

namespace TicketBooking.Model.Models
{
    public class FlightRequestModel
    {
        [Required]
        public Guid Id { set; get; }
        [Required]
        public Guid AircraftId { set; get; }
        public Aircraft? Aircraft { set; get; }
        [Required]
        public int TotalSeat { set; get; }
        [Required]
        public int RemainingSeat { set; get; }
        [Required]
        public bool IsFlightActive { set; get; }
        [Required]
        public int DefaultBaggage { set; get; }
        public decimal BusinessPrice { set; get; }
        public decimal EconomyPrice { set; get; }
        [Required]
        public Guid ScheduleId { set; get; }
        public FlightSchedule? Schedule { set; get; }
        public ICollection<BookingList> BookingLists { get; set; }
    }
}
