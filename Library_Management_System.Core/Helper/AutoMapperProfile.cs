using AutoMapper;
using Library_Management_System.Core.Models;
using Library_Management_System.Core.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, AuthResponse>();
        }
    }
}
