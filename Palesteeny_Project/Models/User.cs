using System.ComponentModel.DataAnnotations;

namespace Palesteeny_Project.Models
{
    public class User

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

      
      
      
  
        public bool EmailConfirmed { get; set; } = false;
        public string? ConfirmationToken { get; set; }

        
        public string Role { get; set; } = "UserPal";
    }
}
