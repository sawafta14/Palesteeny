namespace Palesteeny_Project.Models
{
    public class Template
    {

        public int Id { get; set; }
        public string? Category { get; set; }  // خرائط - معالم - عشوائيات
        public string? ImageUrl { get; set; }  // مسار صورة القالب أو URL
        public virtual ICollection<Drawing> Drawings { get; set; } = new List<Drawing>();
    }
}
