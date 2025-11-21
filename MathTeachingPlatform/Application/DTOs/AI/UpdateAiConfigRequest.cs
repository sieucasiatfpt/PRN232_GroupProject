namespace Application.DTOs.AI
{
    public class UpdateAiConfigRequest
    {
        public string? config_name { get; set; }
        public string? model_name { get; set; }
        public decimal? temperature { get; set; }
        public int? max_tokens { get; set; }
        public string? settings_json { get; set; }
        public bool? is_active { get; set; }
    }
}