﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> RequestBooking([FromBody] BookingRequestModel model)
        {
            if (ModelState.IsValid)
            {
                logger.LogInformation("Start Booking");
                var result = await bookingService.RequestBooking(model);
                if (result.Status !=null)
                    return StatusCode(StatusCodes.Status201Created, Ok(result));
                logger.LogError("Booking failed");
                return BadRequest(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("request-service")]
        public async Task<IActionResult> RequestService(List<Guid> extraServices, Guid bookingList)
        {
            if (ModelState.IsValid)
            {

                logger.LogInformation("Add more service");
                var result = await bookingService.AddBookingService(extraServices, bookingList);
                if (result.Status == true)
                    return StatusCode(StatusCodes.Status201Created, Ok(result));
                logger.LogError("Add service failed");
                return BadRequest(result);
            }
            return BadRequest(ModelState);
        }


        [HttpPost("cancel-booking")]
        public async Task<IActionResult> CancelBooking(Guid bookingId)
        {
            if (ModelState.IsValid)
            {
                logger.LogInformation("Cancel booking");
                var result = await bookingService.CancelBooking(bookingId);
                if (result.Status == true)
                    return StatusCode(StatusCodes.Status201Created, Ok(result));
                logger.LogError("Cancel failed");
                return BadRequest(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("request-contact")]
        public async Task<IActionResult> CreateContact([FromBody] ContactViewModel contact)
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
