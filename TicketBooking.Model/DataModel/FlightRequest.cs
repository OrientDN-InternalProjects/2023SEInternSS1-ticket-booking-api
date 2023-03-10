﻿using System.ComponentModel.DataAnnotations;

namespace TicketBooking.Model.DataModel
{
    public class FlightRequest
    {
        [Required]
        public string DepartCode { set; get; }

        [Required]
        public string ArrivalCode { set; get; }

        [Required]
        public DateTime DepartDate { set; get; }


    }
}
