using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Library_Management_System.Core.Helper;
using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Web.Helper;
using Library_Management_System.Web.Services.Intrerface;
using Library_Management_System.Web.Services.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Library_Management_System.Web.Controllers
{
    public class BookController : BaseController
    {
        private readonly ApiRequestUri _apiRequestUri;
        private readonly IHttpContextAccessor _HttpContext;
        private readonly IResources _resourses;



        public BookController(IOptionsSnapshot<ApiRequestUri> options, IHttpContextAccessor _httpContext, IResources resources) : base(_httpContext.HttpContext)
        {
            _apiRequestUri = options.Value;
            _HttpContext = _httpContext;
            _resourses = resources;

        }
       // [Authorize(Roles = Role.Admin)]
        public ActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                
                
                BookVM bookVM = new BookVM();
                if (this.ModelState.IsValid)
                    using (var httpClient = new HttpClient())
                    {

                            httpClient.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                            HttpContext.Session.GetString("token"));

                        var uri = _apiRequestUri.BaseUri + _apiRequestUri.GetBooks;

                        HttpResponseMessage res = await httpClient.GetAsync(uri);

                        if (res.IsSuccessStatusCode)
                        {
                            var apiTask = res.Content.ReadAsStringAsync();
                            var responseString = apiTask.Result;
                            var model = JsonConvert.DeserializeObject<List<BookVMList>>(responseString);
                            if (res.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                ViewBag.Message = "Unauthorized!";
                            }
                            else
                            {
                                return View(model);
                            }

                                //var model = JsonConvert.DeserializeObject<StudentDto>(responseString); 
                                ////to save info in session
                                //HttpContext.Session.SetString("StudentId", model.StudentId.ToString());

                                //to get saved info from session
                                //string studentId = HttpContext.Session.GetString("StudentId");

                               
                        }
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddBook(AddBookVM bookVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(bookVM);
                }
               // var roles = GetRoles();

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                   
                    var book = new AddBookVM()
                    {
                        BookCode = bookVM.BookCode, 
                        BookTitle = bookVM.BookTitle
                    };

                    var uri = string.Format(_apiRequestUri.AddBook, book);

                    StringContent content = new StringContent(JsonConvert.SerializeObject(bookVM));

                    //HttpResponseMessage response = await httpClient.PostAsync(uri, content);
                    HttpResponseMessage response = (HttpResponseMessage)null;

                    response = await httpClient.PostAsJsonAsync(uri, bookVM);
                    if (response.IsSuccessStatusCode)
                    {

                        return RedirectToAction("GetBooks");
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

        [HttpGet]
        //[Route("SelectCourse")]
        public async Task<IActionResult> SelectCourse()
        
        {
            try
            {           
                //ISession session = HttpContextAccessor.HttpContext.Session;

                //session.SetString("Id",)
                var allBooks = await _resourses.GetBooks();

                ViewBag.BookList = allBooks;

                var userId = GetUserId();

                HttpContext.Session.SetString("Id", userId);
             
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
       // [Route("SelectCourse")]
        public async Task<IActionResult> SelectCourse(int bookId)
        {
            try
            {

                var userId = GetUserId();

                var allBooks = await _resourses.GetBooks();

                ViewBag.BookList = allBooks;

                if (!ModelState.IsValid)
                {
                    return View();
                }

                
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var newBook = new AddUserBookVM()
                    {
                        BookId = bookId,
                        ApplicationUserId = userId

                    };
                    var uri = string.Format(_apiRequestUri.AddUserBooks, userId, bookId);

                   // StringContent content = new StringContent(JsonConvert.SerializeObject(newBook));

                    //HttpResponseMessage response = await httpClient.PostAsync(uri, content);
                    HttpResponseMessage response = (HttpResponseMessage)null;

                    response = await httpClient.PostAsJsonAsync(uri,newBook);
                    if (response.IsSuccessStatusCode)
                    {

                        return RedirectToAction("GetBooksForUser", "UserBooksWeb",userId);
                        //return RedirectToAction("GetStudentById", new { id = student.ID });
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ModelState.AddModelError("", await response.Content.ReadAsStringAsync());
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

