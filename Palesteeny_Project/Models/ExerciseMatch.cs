namespace Palesteeny_Project.Models
{
    public class ExerciseMatch
    {
        public int ExerciseMatchId { get; set; }

        // The option text for matching
        public string OptionText { get; set; } = string.Empty;

        // The label to match against
        public string MatchLabel { get; set; } = string.Empty;

        // Optional image URL for the match
        public string MatchImageUrl { get; set; } = string.Empty;
        public string? OptionImageUrl { get; set; }

        // Foreign key back to ExerciseQuestion
        public int ExerciseQuestionId { get; set; }
        public ExerciseQuestion? ExerciseQuestion { get; set; }
    }
}
