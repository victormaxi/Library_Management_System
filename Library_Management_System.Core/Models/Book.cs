using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Models
{
   public class Book
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public BookAuthor BookAuthors { get; set; }
    }
}
