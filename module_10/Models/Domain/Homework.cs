using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class Homework
    {
        public int Id { get; set; }
        public int LectionId { get; set; }
        public int StudentId { get; set; }
        public Mark Mark { get; set; }
    }
}
