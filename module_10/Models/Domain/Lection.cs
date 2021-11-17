using System;

namespace Models.Domain
{
    public class Lection
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public Date Date { get; set; }
        public int LectorId { get; set; }
    }
}
