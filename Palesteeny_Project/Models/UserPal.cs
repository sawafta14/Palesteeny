using System.ComponentModel.DataAnnotations;
namespace Palesteeny_Project.Models
{
    public class UserPal
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [Display(Name = "الاسم الأول")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "الاسم الثاني مطلوب")]
        [Display(Name = "الاسم الثاني")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        [Display(Name = "البريد الإلكتروني")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [Display(Name = "كلمة المرور")]
        public required string PasswordHash { get; set; }

        [Required(ErrorMessage = "الجنس مطلوب")]
        [Display(Name = "الجنس")]
        public required string Gender { get; set; }

        [Required(ErrorMessage = "الصف مطلوب")]
        [Display(Name = "الصف")]
        public required string Grade { get; set; }

        [Range(3, 100, ErrorMessage = "العمر يجب أن يكون بين 3 و 100")]
        [Display(Name = "العمر")]
        public int? Age { get; set; }

        [Display(Name = "الصورة")]
        public string? ImagePath { get; set; }

        public bool EmailConfirmed { get; set; } = false;
        public string? ConfirmationToken { get; set; }

        // ✅ علاقات جديدة
        public List<FavoriteStory> FavoriteStories { get; set; } = new();
        public List<StoryBookmark> StoryBookmarks { get; set; } = new();
        public List<Drawing> Drawings { get; set; } = new();
    }
}