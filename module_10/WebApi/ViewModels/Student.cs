using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class Student
    {
        public int Id { get; set; }
        public string Fio { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
