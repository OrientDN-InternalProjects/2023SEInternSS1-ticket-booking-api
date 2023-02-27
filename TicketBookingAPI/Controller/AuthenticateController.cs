using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketBooking.Model.AuthenticateModel;
using TicketBooking.Service.AuthenticateService;

namespace TicketBookingAPI.Controller
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
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUp model, bool IsAdmin)
        {
            var result = await accountService.SignUp(model, IsAdmin);
            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignIn model)
        {
            var result = await accountService.SignIn(model);
            if (result != null)
            {
                return Ok(result);
            }
            return Unauthorized();
        }
        [HttpPost("renew-token")]
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