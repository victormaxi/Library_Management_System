using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public BookAuthor BookAuthors { get; set; }
       
    }
}
