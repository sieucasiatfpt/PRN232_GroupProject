using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ai_call_logs")]
    public class AICallLog
    {
        [Key]
        [Column("log_id")]
        public int LogId { get; set; }

        [Required]
        [Column("config_id")]
        public int ConfigId { get; set; }

        [Column("student_id")]
        public int? StudentId { get; set; }

        [Column("matrix_id")]
        public int? MatrixId { get; set; }

        [MaxLength(100)]
        [Column("service_name")]
        public string? ServiceName { get; set; }

        [Column("request_text")]
        public string? RequestText { get; set; }

        [Column("request_data")]
        public string? RequestData { get; set; }

        [Column("response_text")]
        public string? ResponseText { get; set; }

        [Column("response_data")]
        public string? ResponseData { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        // Navigation Properties
        [ForeignKey("ConfigId")]
        public virtual AIConfig AIConfig { get; set; } = null!;

        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }

        [ForeignKey("MatrixId")]
        public virtual ExamMatrix? ExamMatrix { get; set; }
    }
}
