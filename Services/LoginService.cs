using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MMS.Models.ViewModels;
using MMS.Services.Responses;
using System.Reflection;
using System.Security.Claims;

namespace MMS.Services
{
    public class LoginService
    {
        public async Task<ServiceResponse> Login(HttpContext httpContext, LoginViewModel model)
        {
            string correctEmail = "admin";
            string correctPassword = "admin";

            if (model.Email != correctEmail || model.Password != correctPassword)
            {
                return new ServiceResponse(false, "Pogrešan email ili lozinka");
            }

            LoginService authenticationService = new LoginService();
            await Authenticate(httpContext, model);
            return new ServiceResponse(true, "Prijava uspješna");
        }

		private async Task Authenticate(HttpContext httpContext, LoginViewModel model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await httpContext.SignInAsync(principal);
        }

        public async Task<ServiceResponse> Logout(HttpContext httpContext)
        {
			if (!httpContext.User.Identity.IsAuthenticated)
			{
                return new ServiceResponse(false, "Korisnik je već odjavljen");
			}
			await httpContext.SignOutAsync();
            return new ServiceResponse(true, "Odjavljivanje uspješno");
		}
    }
}
