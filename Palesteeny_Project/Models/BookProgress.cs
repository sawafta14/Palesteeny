namespace Palesteeny_Project.Models
{
    public class BookProgress
    {
        public int BookProgressId { get; set; }

        public int UserPalId { get; set; }
        public UserPal UserPal { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int ProgressPercent { get; set; } // 0 to 100
    }

}
