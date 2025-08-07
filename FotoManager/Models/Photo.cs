using System.ComponentModel.DataAnnotations;

namespace FotoManager.Models
{
    public class Photo
    {
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string? Filter { get; set; }  // ex: "grayscale"
        public string? Frame { get; set; }   // ex: "frame-gold"
        public int? BookId { get; set; }
        public Book Book { get; set; }
    }
}
