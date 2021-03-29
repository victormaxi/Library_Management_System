using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.ViewModels
{
    public class BookVM
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookCode { get; set; }
       // public virtual IEnumerable<SelectedBookVM> SelectedBookVMs { get; set; }
    }
}
