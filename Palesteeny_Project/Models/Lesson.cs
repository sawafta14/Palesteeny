namespace Palesteeny_Project.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }

        public string? LessonName { get; set; }

        

        public int StartPage { get; set; } 

        public string? VideoUrl { get; set; } 

        public int BookId { get; set; }
        public Book? Book { get; set; }

        public ICollection<QuestionGroup> QuestionGroups { get; set; } = new List<QuestionGroup>();
        public ICollection<UserLesson> UserLessons { get; set; } = new List<UserLesson>();
    }
}
