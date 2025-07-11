namespace Palesteeny_Project.Models
{
    public class QuizOption
    {
        public int Id { get; set; }
        public string? OptionText { get; set; }
        public bool IsCorrect { get; set; }

        public int QuizQuestionId { get; set; }
        public QuizQuestion? QuizQuestion { get; set; } 
    }
}
