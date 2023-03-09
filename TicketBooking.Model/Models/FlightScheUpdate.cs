using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;

namespace TicketBooking.Model.Models
{
    public class FlightScheUpdate
    {
        public Guid Id { get; set;  }
        
        public DateTime DepartureTime { get; set; }
        
        public DateTime ArrivalTime { get; set; }
    }
}