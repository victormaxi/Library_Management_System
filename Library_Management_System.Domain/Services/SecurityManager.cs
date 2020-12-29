using Library_Management_System.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Library_Management_System.Domain.Services
{
    public class SecurityManager
    {
        //public async void SignIn(HttpContext httpContext)
        //{
        //    ClaimsIdentity claimsIdentity = new ClaimsIdentity(getUserClaims(applicationUser), CookieAuthenticationDefaults.AuthenticationScheme);
        //    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        //    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        //}

        private IEnumerable<Claim> getUserClaims(ApplicationUser applicationUser)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, applicationUser.UserName));
            return claims;
        }

        //private IEnumerable<Claim> getUserClaims(ApplicationUser applicationUser)
        //{
        //    List<Claim> claims = new List<Claim>();
        //    claims.Add(new Claim(ClaimTypes.Name, applicationUser.UserName));
        //    return claims;
        //}
    }
}
