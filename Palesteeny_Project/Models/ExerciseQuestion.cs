namespace Palesteeny_Project.Models
{
    public class ExerciseQuestion
    {
        public int ExerciseQuestionId { get; set; }

        // Question text
        public string? Question { get; set; }

        public int? QuestionGroupId { get; set; }
        public QuestionGroup? QuestionGroup { get; set; }

        // Navigation collections
        public ICollection<ExerciseOption> Options { get; set; } = new List<ExerciseOption>();
        public ICollection<ExerciseMatch> Matches { get; set; } = new List<ExerciseMatch>();
    }
}