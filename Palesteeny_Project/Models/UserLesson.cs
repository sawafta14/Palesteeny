namespace Palesteeny_Project.Models
{
    public class UserLesson
    {
        public int Id { get; set; }

        public int UserPalId { get; set; }
        public UserPal? UserPal { get; set; }

        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int? BookmarkPage { get; set; } 
    }
}
