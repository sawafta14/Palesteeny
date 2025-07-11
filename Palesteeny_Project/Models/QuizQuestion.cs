using Microsoft.VisualBasic.FileIO;

namespace Palesteeny_Project.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string? QuestionText { get; set; }
        public string? ImageUrl { get; set; }
        public int Score { get; set; }
        public string? Category { get; set; } 
        public ICollection<QuizOption> Options { get; set; } = new List<QuizOption>();
    }
}
