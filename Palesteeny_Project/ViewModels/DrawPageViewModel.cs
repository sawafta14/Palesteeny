using Palesteeny_Project.Models;

namespace Palesteeny_Project.ViewModels
{
    public class DrawPageViewModel
    {
        public DrawingDto DrawingDto { get; set; } = new DrawingDto();
        public List<Template> Templates { get; set; } = new();
        public List<string> Categories { get; set; } = new();
    }
}
