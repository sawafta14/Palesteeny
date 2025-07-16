namespace Palesteeny_Project.Models
{
    public class ExerciseOption
    {
        public int ExerciseOptionId { get; set; }

        // The text of the option
        public string OptionQuestion { get; set; } = string.Empty;
        public string? OptionImageUrl { get; set; }

        // True if this option is correct
        public string? Answer { get; set; }

        // Foreign key back to ExerciseQuestion
        public int ExerciseQuestionId { get; set; }
        public ExerciseQuestion? ExerciseQuestion { get; set; }
    }
}
