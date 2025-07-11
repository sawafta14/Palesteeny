namespace Palesteeny_Project.Models
{
    public class ExerciseQuestion
    {
        public int ExerciseQuestionId { get; set; }

        public string? Question { get; set; }
        public string? Type  { get; set; }
        public string? ImageUrl { get; set; }
       
        public bool? questionOverlay { get; set; }
        public string? Answer { get; set; }
        public string? UserAnswer { get; set; }
        public bool? IsCorrect { get; set; }

        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }
    }
}
