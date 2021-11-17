
namespace Models.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string Fio { get; set; }
        public Email Email { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
    }
}
