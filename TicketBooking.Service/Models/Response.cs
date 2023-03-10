using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<<< HEAD:TicketBooking.Service/Models/Response.cs
namespace TicketBooking.Service.Models
========
namespace TicketBooking.Model.Models
>>>>>>>> 4c77dd3c2a05ba0a4b64f6ae8e7252d4e9e88921:TicketBooking.Model/Models/Response.cs
{
    public class Response
    {
        public bool? Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public IEnumerable<object>? List { get; set; } 
    }
        
}
