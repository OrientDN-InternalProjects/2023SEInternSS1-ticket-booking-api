using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketBooking.Data;

namespace TicketBooking.Service.Model
{
    public class AircraftViewModel
    {
        public Guid Id { get; set; }
        public string? Model { get; set; }
        public string? Manufacture { get; set; }
    }
}