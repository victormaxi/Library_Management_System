﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Web.Helper;
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



        public AccountController(IOptionsSnapshot<ApiRequestUri> options, IHttpContextAccessor _httpContext) : base(_httpContext.HttpContext)
        {
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
        public async Task<IActionResult> Login ([FromBody] LoginVM loginVm)
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

                        return RedirectToAction("Book", "GetBooks");
                        //return RedirectToAction("GetStudentById", new { id = student.ID });
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




                    var uri = string.Format(_apiRequestUri.ConfirmEmail, userId, token );
                   
                    HttpResponseMessage response = (HttpResponseMessage)null;

                    response = await  httpClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {

                        return RedirectToAction("EmailConfirmed");
                        //return RedirectToAction("GetStudentById", new { id = student.ID });
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