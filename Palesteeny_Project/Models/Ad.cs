namespace Palesteeny_Project.Models
{
    public class Ad
    {
        public int Id { get; set; }

        public string? Title { get; set; } // يمكن عرضها كـ alt أو Tooltip

        public string ImageUrl { get; set; } = string.Empty;

        public string? Link { get; set; } // عند الضغط على الإعلان

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
