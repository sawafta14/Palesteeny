using Palesteeny_Project.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Palesteeny_Project.Models
{
    public class FavoriteStory
    {
        [Key]
        public int Id { get; set; }

        public int UserPalId { get; set; }
        [ForeignKey("UserPalId")]
        public UserPal? User { get; set; } = null!;

        public int QusId { get; set; }
        [ForeignKey("QusId")]
        public Qusi? Qusi { get; set; } = null!;
    }
}