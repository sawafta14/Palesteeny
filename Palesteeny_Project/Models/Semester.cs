namespace Palesteeny_Project.Models
{
    public class Semester
    {
        public int SemesterId { get; set; }

        

        public string? GradeName { get; set; }
        public string? SemesterName { get; set; } 

        public DateTime? RegistrationDate { get; set; }

        public ICollection<UserPal> UsersPal { get; set; } = new List<UserPal>();
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
