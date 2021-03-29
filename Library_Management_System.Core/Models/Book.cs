using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library_Management_System.Core.Models
{
   public class Book
    {
        
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookCode { get; set; }
        public BookAuthor BookAuthors { get; set; }
        //public virtual IEnumerable<Borrowed_Book> Borrowed_Books { get; set; }
        public virtual List<UserBooks> UserBooks { get; set; }
    }
}
