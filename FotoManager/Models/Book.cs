using System.ComponentModel.DataAnnotations;

namespace FotoManager.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
