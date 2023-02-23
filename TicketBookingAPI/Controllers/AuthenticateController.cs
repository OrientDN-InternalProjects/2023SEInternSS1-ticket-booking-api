using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketBooking.Model.AuthenticateModel;
using TicketBooking.Service.AuthenticateService;

namespace TicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService accountService;

        public AuthenticateController(IAuthenticateService account)
        {
            accountService = account;
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUp model)
        {
            var result = await accountService.SignUp(model);
            if (result!=null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignIn model)
        {
            var result = await accountService.SignIn(model);
            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }
        [HttpPost("SignUpAdmin")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SignUpForAdmin(SignUp model)
        {
            var result = await accountService.SignUpForAdmin(model);
            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }
        [HttpPost("Renew")]
        public async Task<IActionResult> RenewToken(TokenResponse model)
        {
            var result = await accountService.RenewToken(model);
            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }
    }
}
