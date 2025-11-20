namespace Application.DTOs.AI
{
    public class AiCallLogDto
    {
        public int log_id { get; set; }
        public int config_id { get; set; }
        public int? student_id { get; set; }
        public int? matrix_id { get; set; }
        public string? service_name { get; set; }
        public string? request_text { get; set; }
        public string? response_text { get; set; }
        public DateTime created_at { get; set; }
    }
}