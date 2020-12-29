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
        Task AddBook(BookVM book);
        Task<object> GetBooks();
        Task<object> GetBookById(int BookId);
    }
}
