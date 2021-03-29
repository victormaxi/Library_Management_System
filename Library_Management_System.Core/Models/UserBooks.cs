using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Models
{
    public class UserBooks
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User{ get; set; }
    }
}
