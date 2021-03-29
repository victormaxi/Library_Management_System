using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.ViewModels
{
    public class User_BookVM
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public BookVM BookVM { get; set; }
        public SelectedBookVM selectedBookVM { get; set; }
    }
}
