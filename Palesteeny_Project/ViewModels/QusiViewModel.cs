using Palesteeny_Project.Models;

namespace Palesteeny_Project.ViewModels
{
    public class QusiViewMode
    {
        public Qusi Qusi { get; set; } = null!; 
        public string? UserId { get; set; }    
        public bool IsFavorite { get; set; }    
        public int BookmarkOrder { get; set; }  

    }
}
