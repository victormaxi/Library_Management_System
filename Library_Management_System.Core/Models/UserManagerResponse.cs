using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Models
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime? ExpireDate { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
