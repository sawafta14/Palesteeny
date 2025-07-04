namespace Palesteeny_Project.Models
{
    public class Story
    {
   
            public int Id { get; set; }

           
            public string? Title { get; set; }

            public string? Content { get; set; }

            public string? CoverImage { get; set; }

            public string? Category { get; set; }

            public DateTime? CreatedAt { get; set; } = DateTime.Now;
     

}
}
