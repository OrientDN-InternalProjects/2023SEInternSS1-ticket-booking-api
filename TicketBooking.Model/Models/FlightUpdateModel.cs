using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Data.DataModel;

namespace TicketBooking.Model.Models
{
    public class FlightUpdateModel
    {
        public Guid Id { set; get; }
        
        public Guid AircraftId { set; get; }
        
        public int DefaultBaggage { set; get; }

        public decimal BusinessPrice { set; get; }

        public decimal EconomyPrice { set; get; }
        
        public FlightScheUpdate FlightSche { set; get; }
    } 
}

