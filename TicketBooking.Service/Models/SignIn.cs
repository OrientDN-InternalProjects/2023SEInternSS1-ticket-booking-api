using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:TicketBooking.Service/Models/SignIn.cs
namespace TicketBooking.Service.Models
========
namespace TicketBooking.Model.Models
>>>>>>>> 4c77dd3c2a05ba0a4b64f6ae8e7252d4e9e88921:TicketBooking.Model/Models/SignIn.cs
{
    public class SignIn
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
