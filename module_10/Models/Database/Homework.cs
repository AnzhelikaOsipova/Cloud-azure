
using System.ComponentModel.DataAnnotations;

namespace Models.Database
{
    public class Homework : IHasIdProperty<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int LectionId { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int Mark { get; set; }
    }
}
