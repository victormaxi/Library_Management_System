using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_Management_System.Core.Interfaces;
using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBooksController : ControllerBase
    {
        private readonly IUserBooksServices _userBooks;
        public UserBooksController(IUserBooksServices userBooks)
        {
            _userBooks = userBooks;
        }

        [HttpPost]
        [Route("AddUserBooks/{ApplicationUserId}/{BookId}")]
        public async Task<ActionResult<object>> AddUserBooks(AddUserBookVM bookVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newBook = new UserBooks()
                    {
                        ApplicationUserId = bookVM.ApplicationUserId,
                        BookId = bookVM.BookId
                    };
                    var result = await _userBooks.AddUserBook(newBook);
                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetBooksForUser/{userId}")]
        public async Task<ActionResult<IEnumerable<BookVM>>> GetBooksForUser(string userId)
        {
            try
            {

                var result = await _userBooks.GetBookForUser(userId);
                
                return Ok(result);
     
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
