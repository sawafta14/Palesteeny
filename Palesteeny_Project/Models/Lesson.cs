namespace Palesteeny_Project.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }

        public string? LessonName { get; set; }

        

        public int StartPage { get; set; } // رقم الصفحة التي يبدأ منها الدرس

        public string? VideoUrl { get; set; } // رابط فيديو الدرس

        public int BookId { get; set; }
        public Book? Book { get; set; }

        public ICollection<ExerciseQuestion> ExerciseQuestions { get; set; } = new List<ExerciseQuestion>();
        public ICollection<UserLesson> UserLessons { get; set; } = new List<UserLesson>();
    }
}
