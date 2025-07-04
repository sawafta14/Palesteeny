using Palesteeny_Project.Models;
using System.ComponentModel.DataAnnotations;
namespace Palesteeny_Project.Models
{
    public class Qusi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "عنوان القصة")]
        public string? Title { get; set; }

        [Display(Name = "التصنيف")]
        public string? Category { get; set; }

        [Display(Name = "رابط الصورة")]
        public string? CoverImage { get; set; }

        [Display(Name = "رابط القصة")]
        public string? Link { get; set; }

        public List<QusiImage> Images { get; set; } = new();
    }
}