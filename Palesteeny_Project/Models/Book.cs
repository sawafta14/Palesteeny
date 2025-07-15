namespace Palesteeny_Project.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string? Title { get; set; }

        public string? PdfUrl { get; set; } 
        public int NumberOfLessons { get; set; }
        public int SemesterId { get; set; }
        public Semester? Semester { get; set; }

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}
