using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:TicketBooking.Service/Models/SignUp.cs
namespace TicketBooking.Service.Models
========
namespace TicketBooking.Model.Models
>>>>>>>> 4c77dd3c2a05ba0a4b64f6ae8e7252d4e9e88921:TicketBooking.Model/Models/SignUp.cs
{
    public class SignUp
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; } 
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public bool IsAdmin { get; set; }
    }
}
