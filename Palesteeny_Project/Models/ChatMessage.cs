namespace Palesteeny_Project.Models
{
    public class ChatMessage
    {
        public string UserMessage { get; set; }
        public string CurrentPage { get; set; } // e.g., "book", "quiz", "games"
        public string UserId { get; set; } // optional if logged-in user is handled via claims
    }
}
