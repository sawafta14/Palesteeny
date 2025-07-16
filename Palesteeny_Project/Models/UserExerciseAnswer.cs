namespace Palesteeny_Project.Models
{
    public class UserExerciseAnswer
    {
        public int Id { get; set; }

        public string? UserId { get; set; }  // أو UserPalId لو تستخدم نظامك

        public int ExerciseOptionId { get; set; }
        public ExerciseOption? ExerciseOption { get; set; }

        public string? UserAnswer { get; set; }

        public bool? IsCorrect { get; set; }
    }
}
