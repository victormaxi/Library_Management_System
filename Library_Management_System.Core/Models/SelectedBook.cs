using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Models
{
    public class SelectedBook
    {
        public int id { get; set; }
        public string userId { get; set; }
        public int bookId { get; set; }
        public virtual IEnumerable<Borrowed_Book> Borrowed_Books { get; set; }
    }
}
