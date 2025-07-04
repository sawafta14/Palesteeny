using System.ComponentModel.DataAnnotations.Schema;

namespace Palesteeny_Project.Models
{
    public class Drawing
    {
        public int DrawingId { get; set; }

        public int UserPalId { get; set; }
        [ForeignKey("UserPalId")]
        public UserPal? User { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string? Mode { get; set; }  // "free" أو "template"


        public int? TemplateId { get; set; }

        [ForeignKey("TemplateId")]
        public Template? Template { get; set; }

        // ✅ جديد: نوع الفرشاة
        public string? BrushType { get; set; }

        // ✅ جديد: اللون المستخدم
        public string? Color { get; set; }
    }
}
