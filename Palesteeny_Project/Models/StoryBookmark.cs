using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Palesteeny_Project.Models
{
    public class StoryBookmark
    {
        [Key]
        public int Id { get; set; }

        public int UserPalId { get; set; }
        [ForeignKey("UserPalId")]
        public UserPal? User { get; set; } = null!;

        public int QusiId { get; set; }
        [ForeignKey("QusiId")]
        public Qusi? Qusi { get; set; } = null!;

        public int LastImageIndex { get; set; }  // رقم الصورة اللي وقف عندها
    }
}