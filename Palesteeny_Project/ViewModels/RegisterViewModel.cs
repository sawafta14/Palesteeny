using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Palesteeny_Project.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "الاسم الأول مطلوب.")]
        [Display(Name = "الاسم الأول")]
        public   string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "الاسم الثاني مطلوب.")]
        [Display(Name = "الاسم الثاني")]
        public  string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        [Display(Name = "البريد الإلكتروني")]
        public  string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "يجب أن تكون كلمة المرور على الأقل {2} وعلى الأكثر {1} حرفًا.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "يجب أن تحتوي كلمة المرور على حرف صغير، حرف كبير، رقم، ورمز خاص.")]
        [Display(Name = "كلمة المرور")]
        public  string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين.")]
        [Display(Name = "تأكيد كلمة المرور")]
        public  string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "الجنس مطلوب.")]

        [Display(Name = "الجنس")]
        public  string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "الصف مطلوب.")]
        
        [Display(Name = "الصف")]
        public int Grade { get; set; }
        public int SemesterId { get; set; }
        public IEnumerable<SelectListItem> SemesterSelectList { get; set; } = new List<SelectListItem>();
    }
}
