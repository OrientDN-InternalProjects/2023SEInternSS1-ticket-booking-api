using Microsoft.AspNetCore.Mvc;
using TicketBooking.Service.Models;
using TicketBooking.Service.Services.BookingService;
using TicketBooking.Service.Services.ContactDetailService;

namespace TicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService bookingService;
        private readonly IContactDetailServcie contactService;
        private readonly ILogger<BookingController> logger;

        public BookingController(IBookingService bookingService, IContactDetailServcie contactService, ILogger<BookingController> logger)
        {
            this.bookingService = bookingService;
            this.contactService = contactService;
            this.logger = logger;
        }

        [HttpPost("request-booking")]
        public async Task<IActionResult> RequestBooking(BookingRequestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    logger.LogInformation("Start Booking");
                    var result = await bookingService.RequestBooking(model);
                    return StatusCode(StatusCodes.Status201Created, Ok(result));
                }
                catch (Exception e)
                {
                    logger.LogError("Booking failed");
                    return BadRequest(e.Message);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost("request-service")]
        public async Task<IActionResult> RequestService(List<Guid> extraServices, Guid bookingList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    logger.LogInformation("Add more service");
                    var result = await bookingService.AddBookingService(extraServices, bookingList);
                    return StatusCode(StatusCodes.Status201Created, Ok(result));
                }
                catch (Exception e)
                {
                    logger.LogError("Add service failed");
                    return BadRequest(e.Message);
                }
            }
            return BadRequest(ModelState);
        }


        [HttpPost("cancel-booking")]
        public async Task<IActionResult> CancelBooking(Guid bookingId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    logger.LogInformation("Add more service");
                    var result = await bookingService.CancelBooking(bookingId);
                    return StatusCode(StatusCodes.Status201Created, Ok(result));
                }
                catch (Exception e)
                {
                    logger.LogError("Add service failed");
                    return BadRequest(e.Message);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("request-contact")]
        public async Task<IActionResult> CreateContact(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                var result = await contactService.CreateContactInfo(contact);
                if (result != null)
                {
                    return Ok(result);
                }

                return BadRequest("Create failed");

            }
            return BadRequest(ModelState);
        }
    }
}
