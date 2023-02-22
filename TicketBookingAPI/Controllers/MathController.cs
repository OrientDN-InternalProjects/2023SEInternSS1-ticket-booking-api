using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Diagnostics;
using TicketBooking.Common.AppExceptions;

namespace TicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MathController : ControllerBase
    {
        private readonly ILogger<MathController> _logger;
        public MathController(ILogger<MathController> logger)
        {
            _logger = logger;
            _logger.LogInformation("Writing to log file with INFORMATION severity level.");
        
            _logger.LogWarning("Writing to log file with WARNING severity level.");
        
            _logger.LogError("Writing to log file with ERROR severity level.");

}
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
