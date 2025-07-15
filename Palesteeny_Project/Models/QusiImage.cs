namespace Palesteeny_Project.Models
{
    public class QusiImage
    {
        public int Id { get; set; }
        public int QusId { get; set; }
        public string ImageUrl { get; set; } = "";
        public int Order { get; set; }  

        public Qusi? Qusi { get; set; }
    }
}
