﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MMS.Models;
using MMS.Models.ViewModels;
using MMS.Services.Responses;
using System.Reflection;
using System.Security.Claims;

namespace MMS.Services
{
    public class LoginService
    {
        private readonly UserService _userService;

        public LoginService(UserService userService)
        {
            _userService = userService;
        }

        public async Task<LoginServiceResponse> Login(HttpContext httpContext, LoginViewModel model)
        {
            var user = await _userService.GetUserByLoginInformation(model);
            if (user == null)
            {
				return new LoginServiceResponse(false, "", "Pogrešan email ili lozinka");
			}

            await Authenticate(httpContext, user);
            return new LoginServiceResponse(true, user.Role.Name, "Prijava uspješna");
        }

		private async Task Authenticate(HttpContext httpContext, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name + " " + user.Surname),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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
