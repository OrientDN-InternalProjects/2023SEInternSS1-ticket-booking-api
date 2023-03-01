﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketBooking.Model.Models;
using TicketBooking.Service.Models;
using TicketBooking.Service.Services.AuthenticateService;
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

        public BookingController(IBookingService bookingService, IContactDetailServcie contactService)
        {
            this.bookingService = bookingService;
            this.contactService = contactService;
        }

        [HttpPost("request-booking")]
        public async Task<IActionResult> RequestBooking(BookingRequestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await bookingService.RequestBooking(model);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                catch
                {
                    return BadRequest("Booking failed");
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
                    var result = await bookingService.AddBookingService(extraServices, bookingList);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                catch
                {
                    return BadRequest("Booking failed");
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