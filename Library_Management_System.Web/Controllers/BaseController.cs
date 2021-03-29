using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library_Management_System.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly HttpContext context;

        public BaseController(HttpContext context)
        {
            this.context = context;
        }

        // returns the current authenticated account (null if not logged in)
        public ApplicationUser ApplicationUser => (ApplicationUser)HttpContext.Items["ApplicationUser"];

        [NonAction]
        public string GetUserId()
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated == false)
                return string.Empty;
            if (claimsIdentity.FindFirst("Id") != null)
            {
                return claimsIdentity.FindFirst("Id").Value.ToString();
            }
            return string.Empty;
        }

        [NonAction]
        public string GetToken()
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated == false)
                return string.Empty;
            if (claimsIdentity.FindFirst("JWT") != null)
            {

                return claimsIdentity.FindFirst("JWT").Value.ToString();

            }
            return string.Empty;
        }

        public string GetEmail()
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated == false)
                return string.Empty;
            if (claimsIdentity.FindFirst("Email") != null)
            {
                return claimsIdentity.FindFirst("Email").Value.ToString();
            }
            return string.Empty;
        }
        public string GetUserName()
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated == false)
                return string.Empty;
            if (claimsIdentity.FindFirst("FullName") != null)
            {
                return claimsIdentity.FindFirst("FullName").Value.ToString();
            }
            //if(claimsIdentity.FindFirst(context.User.Identity.Name) != null)
            //{
            //    return claimsIdentity.FindFirst(context.User.Identity.Name).Value.ToString();
            //}
            return string.Empty; 
        }

        public List<string> GetRoles()
        {
            var claimsIdentity = context.User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated == false)
                return null;
            var roles = claimsIdentity.Claims;
            List<string> rolesLst = new List<string>();
            foreach (var claim in roles)
            {
                rolesLst.Add(claim.Value);
            }
            return rolesLst;
        }

    }
}
