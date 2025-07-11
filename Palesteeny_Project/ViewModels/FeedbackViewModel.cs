using System.ComponentModel.DataAnnotations;

namespace Palesteeny_Project.ViewModels
{
    public class FeedbackViewModel
    {
       
        public string? Name { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? Message { get; set; }
    }
}
