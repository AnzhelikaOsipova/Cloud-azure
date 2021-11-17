
using System.ComponentModel.DataAnnotations;

namespace Models.Database
{
    public class Lection : IHasIdProperty<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Topic { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public int LectorId { get; set; }
    }
}
