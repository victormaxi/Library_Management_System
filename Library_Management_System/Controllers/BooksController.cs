using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_Management_System.Core.Interfaces;
using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookServices bookServices;
        public BooksController(IBookServices bookServices)
        {
            this.bookServices = bookServices;
        }
        public Task AddBook(BookVM bookVM)
        {
            try
            {
                var book = bookServices.AddBook(bookVM);
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public Task<object> GetBooks()
        {
            try
            {
                var book = bookServices.GetBooks();
                return book;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
