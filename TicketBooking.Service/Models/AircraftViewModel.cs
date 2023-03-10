using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketBooking.Data;
using System.ComponentModel.DataAnnotations;

<<<<<<<< HEAD:TicketBooking.Service/Models/AircraftViewModel.cs
namespace TicketBooking.Service.Models
========
namespace TicketBooking.Model.Models
>>>>>>>> 4c77dd3c2a05ba0a4b64f6ae8e7252d4e9e88921:TicketBooking.Model/Models/AircraftViewModel.cs
{
    public class AircraftViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(6)]
        public string Model { get; set; }

        [Required]
        [MaxLength(6)]
        public string Manufacture { get; set; }

        [Required]
        public int NumRowSeat { get; set; }

        [Required]
        public int NumColumnSeat { get; set; }

        [Required]
        public int NumRowBusiness { get; set; }

        [Required]
        public int NumRowEconomy { get; set; }
    }
}