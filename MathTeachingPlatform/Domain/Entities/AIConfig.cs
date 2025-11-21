using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ai_configs")]
    public class AIConfig
    {
        [Key]
        [Column("config_id")]
        public int ConfigId { get; set; }

        [Required]
        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("config_name")]
        public string ConfigName { get; set; } = string.Empty;

        [MaxLength(100)]
        [Column("model_name")]
        public string? ModelName { get; set; }

        [Column("temperature", TypeName = "decimal(3,2)")]
        public decimal? Temperature { get; set; }

        [Column("max_tokens")]
        public int? MaxTokens { get; set; }

        [Column("config_details")]
        public string? ConfigDetails { get; set; }

        [Column("settings_json")]
        public string? SettingsJson { get; set; }

        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public virtual ICollection<AICallLog> AICallLogs { get; set; } = new List<AICallLog>();
    }
}
