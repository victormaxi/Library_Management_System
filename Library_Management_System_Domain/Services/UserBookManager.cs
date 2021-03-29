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

namespace Library_Management_System_Domain.Services
{
    public class UserBookManager : IUserBooksServices
    {
        private readonly ApplicationDBContext _context;
        public UserBookManager(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<object> AddUserBook(UserBooks userBooks)
        {
            try
            {
                if(userBooks != null)
                {
                    var result = await _context.UserBooks.AddAsync(userBooks);
                    var save = await _context.SaveChangesAsync();

                    if(save == 1)
                    {
                        return result;
                    }
                    
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<object> GetBookForUser(string userId)
        {
            try
            {
                 var result = await _context.UserBooks.Include(c => c.Book).Where(p => p.User.Id == userId).Select(c => new BookVM()
                 {
                     BookId = c.BookId,
                     BookCode = c.Book.BookCode,
                     BookTitle = c.Book.BookTitle
                 }).AsQueryable() .ToListAsync();

                //var result = await _context.Users.Include(c => c.UserBooks).ThenInclude(x => x.Book).SingleOrDefaultAsync(m => m.Id == userId) ;
                if(result == null)
                {
                    return string.Empty;
                }
                else
                {
                    return result;
                }
               // return result;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<object> GetUserBooks()
        {
            throw new NotImplementedException();
        }
    }
}
