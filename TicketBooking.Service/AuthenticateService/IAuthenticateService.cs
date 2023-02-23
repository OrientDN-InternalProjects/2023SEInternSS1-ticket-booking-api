using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBooking.Model.AuthenticateModel;

namespace TicketBooking.Service.AuthenticateService
{
    public interface IAuthenticateService
    {
        public Task<Response> SignUp(SignUp model);
        public Task<Response> SignIn(SignIn model);
        public Task<Response> SignUpForAdmin (SignUp model);
        public Task<Response> RenewToken(TokenResponse token);
    }
}
