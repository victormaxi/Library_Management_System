using Library_Management_System.Core.Interfaces;
using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;

using Library_Management_System_Data;
using Microsoft.EntityFrameworkCore;
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
        public async Task<object> AddBook (BookVM bookVM)
        {
            try
            {
                if (bookVM == null)
                {
                    throw new ArgumentNullException();
                }
                var book = new Book()
                {
                    BookCode = bookVM.BookCode,
                    BookTitle = bookVM.BookTitle
                };
                await context.Books.AddAsync(book);
                await context.SaveChangesAsync();
                return book;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<BookVM>> GetBooks()
        {
            try
            {
                var newBook = await context.Books.Select(c => new BookVM()
                {
                    BookId = c.BookId,
                    BookCode = c.BookCode,
                    BookTitle = c.BookTitle

                }).ToListAsync();
                await context.DisposeAsync();
                return newBook;
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

        public async Task<object> SelectCourse(SelectedBook book)
        {
            try
            {
                await context.SelectedBooks.AddAsync(book);
                await context.SaveChangesAsync();

                return book;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> AddUserBoook(User_BookVM book)
        {
            try
            {
                if(book != null)
                {
                    var newBook = new Borrowed_Book()
                    {
                        BookId = book.BookId,
                        UserId = book.UserId,
                        
                        
                       
                    };

                    var saveAsync = await context.Borrowed_Books.AddAsync(newBook);
                    if(saveAsync != null)
                    {
                       await context.SaveChangesAsync();
                    }
                    return saveAsync;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<object> AddUserBook(SelectedBookVM book)
        {
            throw new NotImplementedException();
        }
    }
}
