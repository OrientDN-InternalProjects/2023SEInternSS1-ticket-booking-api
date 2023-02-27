using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Service.Model;

namespace TicketBooking.Service.Services.AuthenticateService
{
    public interface IAuthenticateService
    {
        public Task<Response> SignUp(SignUp model, bool IsAdmin);
        public Task<Response> SignIn(SignIn model);
        public Task<Response> RenewToken(TokenResponse token);
    }
}
