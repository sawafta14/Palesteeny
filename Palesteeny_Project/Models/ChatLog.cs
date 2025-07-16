using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Palesteeny_Project.Models
{
    public class ChatLog
    {
        [Key]
        public int Id { get; set; }

        public int UserPalId { get; set; }

        [ForeignKey("UserPalId")]
        public UserPal? User { get; set; }

        [Required]
        public string? Message { get; set; }

        public string? Reply { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string? PageName { get; set; }

        public int? PageId { get; set; }
    }
}
