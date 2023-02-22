using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketBooking.Common.AppExceptions;

namespace TicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MathController : ControllerBase
    {

        [HttpGet]
        public decimal Divide(decimal Numerator, decimal Denominator)
        {
            try
            {
                return (Numerator / Denominator);
            }catch
            {
                throw new AppException("Parameter is incorrect");
            }
        }
    }

}
