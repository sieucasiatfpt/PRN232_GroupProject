namespace Application.DTOs.AI
{
    public class CreateAiConfigRequest
    {
        public int teacher_id { get; set; }
        public string config_name { get; set; } = string.Empty;
        public string? model_name { get; set; }
        public decimal? temperature { get; set; }
        public int? max_tokens { get; set; }
        public string? settings_json { get; set; }
        public bool is_active { get; set; } = true;
    }
}