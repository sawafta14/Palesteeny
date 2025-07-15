namespace Palesteeny_Project.Models
{
    public class Template
    {

        public int Id { get; set; }
        public string? Category { get; set; }  
        public string? ImageUrl { get; set; }  
        public virtual ICollection<Drawing> Drawings { get; set; } = new List<Drawing>();
    }
}
