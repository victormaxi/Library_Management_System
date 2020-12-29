using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Models
{
    public class BookAuthor
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Books { get; set; }
        public int AuthorId { get; set; }
        public Author Authors{ get; set; }
    }
}
