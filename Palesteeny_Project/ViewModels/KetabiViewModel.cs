using Palesteeny_Project.Models;

namespace Palesteeny_Project.ViewModels
{
    public class KetabiViewModel
    {
        public List<string> Grades { get; set; } = new();
        public List<Semester> Semesters { get; set; } = new();
        public List<Lesson> Lessons { get; set; } = new();
        public int? SelectedLessonId { get; set; }
        public int SemesterId { get; set; }
        public string? PdfUrl { get; set; }
        public int? BookmarkPage { get; set; }
        public int? UserPalId { get; set; }
        public int NumberOfLessons { get; set; }
        public int BookId { get; set; }



    }
}
