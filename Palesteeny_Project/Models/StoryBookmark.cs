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

        public int QusId { get; set; }
        [ForeignKey("QusId")]
        public Qusi? Qusi { get; set; } = null!;

        public int LastImageIndex { get; set; }  
    }
}