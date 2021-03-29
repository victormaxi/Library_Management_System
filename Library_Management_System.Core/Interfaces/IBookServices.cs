using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Core.Interfaces
{
    public interface IBookServices
    {
        Task<object> AddBook(BookVM book);
        Task<List<BookVM>> GetBooks();
        Task<object> GetBookById(int BookId);
        Task<object> SelectCourse(SelectedBook book);
        Task<object> AddUserBook(SelectedBookVM book);
    }
}
