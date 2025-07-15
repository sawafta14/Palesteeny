namespace Palesteeny_Project.Models
{
    public class Ad
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public string? Link { get; set; } 

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
