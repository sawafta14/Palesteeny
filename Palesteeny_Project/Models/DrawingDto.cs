namespace Palesteeny_Project.Models
{
    public class DrawingDto
    {
        public string? Base64Image { get; set; }
        public int UserPalId { get; set; }
        public string? Mode { get; set; } // "free" أو "template"
        public int? TemplateId { get; set; }
        public string? Category { get; set; }
        public string? BrushType { get; set; }
        public string? Color { get; set; }
        public string? TemplateImageUrl { get; set; }

    }
}
 