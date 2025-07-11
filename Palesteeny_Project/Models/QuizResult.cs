using System.ComponentModel.DataAnnotations.Schema;

namespace Palesteeny_Project.Models
{
    public class QuizResult
    {
        public int Id { get; set; }
        public int UserPalId { get; set; }
        [ForeignKey("UserPalId")]
        public UserPal? User { get; set; } = null!;
        public string Category { get; set; } = null!;

        public int TotalScore { get; set; }
        public DateTime TakenAt { get; set; } = DateTime.Now;


    }
}
