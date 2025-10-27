using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ai_history_chats")]
    public class AIHistoryChat
    {
        [Key]
        [Column("chat_id")]
        public int ChatId { get; set; }

        [Required]
        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Required]
        [Column("message")]
        public string Message { get; set; } = string.Empty;

        [Column("chat_summary")]
        public string? ChatSummary { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; } = null!;
    }
}
