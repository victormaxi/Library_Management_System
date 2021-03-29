using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Library_Management_System.Core.Utility;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Web.Helper;
using Library_Management_System.Web.Services.Intrerface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Library_Management_System.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ApiRequestUri _apiRequestUri;
        private readonly IHttpContextAccessor _HttpContext;
        private readonly IResources _resource;
        private readonly IHostingEnvironment _appEnvironment;

       

        public AccountController(IOptionsSnapshot<ApiRequestUri> options, IHttpContextAccessor _httpContext, IResources resource, IHostingEnvironment appEnvironment) : base(_httpContext.HttpContext)
        {
            _resource = resource;
            _appEnvironment = appEnvironment;
            _apiRequestUri = options.Value;
            _HttpContext = _httpContext;

        }
        public IActionResult Index()
        {
            

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUserVM model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(model);
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var user = new ApplicationUserVM()
                    {
                        Email = model.Email,
                        Password = model.Password,
                        ConfirmPassword = model.ConfirmPassword,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.UserName
                    };

                    var uri = string.Format(_apiRequestUri.Register, user);
                   
                    StringContent content = new StringContent(JsonConvert.SerializeObject(model));

                    //HttpResponseMessage response = await httpClient.PostAsync(uri, content);
                    HttpResponseMessage response = (HttpResponseMessage)null;

                    response = await httpClient.PostAsJsonAsync(uri, model);
                    if (response.IsSuccessStatusCode)
                    {

                        return RedirectToAction("Login");
                        //return RedirectToAction("GetStudentById", new { id = student.ID });
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ModelState.AddModelError("", await response.Content.ReadAsStringAsync());
                    }
                }
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login (LoginVM loginVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(loginVm);
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var user = new LoginVM()
                    {
                        
                        
                        Password = loginVm.Password,
                        UserName = loginVm.UserName
                    };

                    var uri = string.Format(_apiRequestUri.Login, user);

                    StringContent content = new StringContent(JsonConvert.SerializeObject(loginVm));

                    //HttpResponseMessage response = await httpClient.PostAsync(uri, content);
                    HttpResponseMessage response = (HttpResponseMessage)null;

                    response = await httpClient.PostAsJsonAsync(uri, loginVm);
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Status = 1;
                        #region cookie Implementation
                        string stringJWT = response.Content.ReadAsStringAsync().Result;
                        // JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);
                        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(stringJWT);

                        var resp = new AuthResponse
                        {
                            userId = authResponse.userId,
                           // userCode = authResponse.userCode.ToString(),
                            Role = authResponse.Role,
                            username = authResponse.Email,
                            Email = authResponse.Email,
                            token = authResponse.token,
                            expiryDate = authResponse.expiryDate
                        };

                     
                        var claims = new List<Claim>
                       // var claims = new []
                        {
                            
                                new Claim("Id", authResponse.userId.ToString()),
                                //new Claim("Code", authResponse.userCode),
                                new Claim(ClaimTypes.Name, loginVm.UserName),
                                new Claim("FullName", "firstname lastname"),
                                new Claim(ClaimTypes.Role, authResponse.Role),
                                new Claim("JWT", authResponse.token),
                                new Claim("Email", authResponse.Email)

                        };

                        string roleVal;

                        //foreach (var role in authResponse.Roles)
                        //{

                        //    if (role == 0)
                        //    {
                        //        roleVal = "Admin";
                        //    }

                        //    else
                        //    {
                        //        roleVal = "Student";
                        //    }

                        //    claims.Add(new Claim(ClaimTypes.Role, roleVal));
                        //}

                        var role = authResponse.Role;
                        if ( role == "Admin")
                        {
                            roleVal = "Admin";
                        }
                        else
                        {
                            roleVal = "Student";
                        }
                        claims.Add(new Claim(ClaimTypes.Role, roleVal));

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            // Refreshing the authentication session should be allowed.

                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                            // The time at which the authentication ticket expires. A 
                            // value set here overrides the ExpireTimeSpan option of 
                            // CookieAuthenticationOptions set with AddCookie.

                            IsPersistent = true,
                            // Whether the authentication session is persisted across 
                            // multiple requests. When used with cookies, controls
                            // whether the cookie's lifetime is absolute (matching the
                            // lifetime of the authentication ticket) or session-based.

                            //IssuedUtc = <DateTimeOffset>,
                            // The time at which the authentication ticket was issued.

                            //RedirectUri = <string>
                            // The full path or absolute URI to be used as an http 
                            // redirect response value.
                        };

                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
                        //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        //HttpContext.Session.SetString("token", jwt.Token);

                        // ViewBag.Message = "User logged in successfully!";
                        if(claimsPrincipal.IsInRole("Admin"))
                        {
                            return RedirectToAction("Index", "AdminDashBoard", new { area = "Admin"});
                        }
                        else if(claimsPrincipal.IsInRole("Student"))
                        {
                            return RedirectToAction("Index", "StudentDashBoard", new { area = "Student" });
                        }
                        
                        // return RedirectToAction("Book", "GetBooks");
                        //return RedirectToAction("GetStudentById", new { id = student.ID });
#endregion
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ModelState.AddModelError("", await response.Content.ReadAsStringAsync());
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("token");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.Message = "User logged out successfully!";
            return View("Login");
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                    httpClient.DefaultRequestHeaders.Accept.Clear(); 
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                     

                     var uri = string.Format(_apiRequestUri.ConfirmEmail, userId, token);
                    
                     
                    HttpResponseMessage response = await httpClient.GetAsync(uri);
                     
                    if (response.IsSuccessStatusCode)
                    {

                        return RedirectToAction("EmailConfirmed");
               
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ModelState.AddModelError("", await response.Content.ReadAsStringAsync());
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return RedirectToAction("EmailConfirmationFailed");
        }
        public ActionResult EmailConfirmed ()
        {
            return View();
        }
        public ActionResult EmailConfirmationFailed()
        {
            return View();
        }
    }
}
