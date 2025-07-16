namespace Palesteeny_Project.Models
{
    public class QuestionGroup
    {
        public int QuestionGroupId { get; set; }

        public string Title { get; set; } = string.Empty; // e.g. "Match the item to its image"

        public string? SharedImageUrl { get; set; } // Optional background image for the group
                                                    
        // Optional overlay flag (nullable bool)
        public bool? QuestionOverlay { get; set; }

        // Question type (e.g., multiple_choice, drag_drop, etc.)
        public string? Type { get; set; }

        // Foreign key to Lesson
        public int? LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        public ICollection<ExerciseQuestion> Questions { get; set; } = new List<ExerciseQuestion>();
    }
}
