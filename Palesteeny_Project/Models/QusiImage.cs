namespace Palesteeny_Project.Models
{
    public class QusiImage
    {
        public int Id { get; set; }
        public int QusiId { get; set; }
        public string ImageUrl { get; set; } = "";
        public int Order { get; set; }  // ترتيب الصورة في العرض

        public Qusi? Qusi { get; set; }
    }
}
