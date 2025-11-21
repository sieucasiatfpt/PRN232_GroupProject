namespace Application.DTOs.AI
{
    public class CreateAiChatRequest
    {
        public int teacher_id { get; set; }
        public string message { get; set; } = string.Empty;
        public string? chat_summary { get; set; }
    }
}