namespace Palesteeny_Project.Models
{
    public class SearchableContent
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public string? Type { get; set; }  // e.g., Book, Story, Game, Map, About
        public string? Description { get; set; }
    }
}
