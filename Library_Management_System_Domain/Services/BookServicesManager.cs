using Library_Management_System.Core.Interfaces;
using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Domain.Services
{
    public class BookServicesManager : IBookServices
    {
        private readonly ApplicationDBContext context;

        public BookServicesManager(ApplicationDBContext context)
        {
            this.context = context;
        }
        public async Task AddBook (BookVM bookVM)
        {
            try
            {
                var book = new Book()
                {
                    BookId = bookVM.BookId,
                    BookTitle = bookVM.BookTitle
                };
                await context.Books.AddAsync(book);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<object> GetBooks()
        {
            try
            {
                return await context.Books.FindAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } 
        public async Task<object> GetBookById (int BookId)
        {
            try
            {
                return await context.Books.FindAsync(BookId);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
