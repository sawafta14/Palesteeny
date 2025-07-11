namespace Palesteeny_Project.Models
{
    public class Admin:User
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
