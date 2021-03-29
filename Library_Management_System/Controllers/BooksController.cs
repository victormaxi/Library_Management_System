using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Library_Management_System.Core.Interfaces;
using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using Library_Management_System_Data;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
   // [Authorize(Roles = Role.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookServices bookServices;
        public BooksController(IBookServices bookServices)
        {
            this.bookServices = bookServices;
        }

        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook(BookVM bookVM)
        {
            try
            {
                var book = bookServices.AddBook(bookVM);
                return Ok(book);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBooks")]
        public async Task<ActionResult<IEnumerable<BookVM>>> GetBooks()
         {
            try
            {
                var book = await bookServices.GetBooks();
                return Ok(book);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Route("SelectCourse")]
        public async Task<IActionResult> SelectCourse(SelectedBookVM book)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return null;
                }
                var bk = await bookServices.AddUserBook(book);
                return Ok(bk);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
