using System.ComponentModel.DataAnnotations;
namespace Palesteeny_Project.Models
{
    public class UserPal:User
    {
     

        [Required(ErrorMessage = "الجنس مطلوب")]
        [Display(Name = "الجنس")]
        public required string Gender { get; set; }

       

        [Range(3, 100, ErrorMessage = "العمر يجب أن يكون بين 3 و 100")]
        [Display(Name = "العمر")]
        public int? Age { get; set; }

        [Display(Name = "الصورة")]
        public string? ImagePath { get; set; }
        public int SemesterId { get; set; }
        public Semester? Semester { get; set; }



        public List<UserLesson> UserLessons { get; set; } = new();


        // ✅ علاقات جديدة
        public List<FavoriteStory> FavoriteStories { get; set; } = new();
        public List<StoryBookmark> StoryBookmarks { get; set; } = new();
        public List<Drawing> Drawings { get; set; } = new();
        public ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();


    }
}