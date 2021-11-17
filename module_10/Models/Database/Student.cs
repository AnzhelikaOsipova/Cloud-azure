using System.ComponentModel.DataAnnotations;

namespace Models.Database
{
    public class Student : IHasIdProperty<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Fio { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
