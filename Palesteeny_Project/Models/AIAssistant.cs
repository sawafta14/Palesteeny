using System.ComponentModel.DataAnnotations;

namespace Palesteeny_Project.Models
{
    public class AIAssistant
    {
        public int Id { get; set; }

        [Required]
        public string? Color { get; set; }

        [Required]
        public string? ImageUrl { get; set; }

        [Required]
        public string? Gender { get; set; }
        public string? Name { get; set; }


        public List<AIAssistant> AIAssistants { get; set; } = new();

    }
}
