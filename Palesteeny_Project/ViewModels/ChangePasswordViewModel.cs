using System.ComponentModel.DataAnnotations;

namespace Palesteeny_Project.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة.")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "يجب أن تكون كلمة المرور على الأقل {2} حرفًا وعلى الأكثر {1} حرفًا.")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الجديدة")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "يجب أن تحتوي كلمة المرور على حرف صغير، حرف كبير، رقم، ورمز خاص.")]
        public  string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "تأكيد كلمة المرور الجديدة مطلوب.")]
        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور الجديدة")]
        [Compare("NewPassword", ErrorMessage = "كلمتا المرور غير متطابقتين.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
