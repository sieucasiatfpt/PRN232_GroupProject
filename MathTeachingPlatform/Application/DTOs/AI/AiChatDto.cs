namespace Application.DTOs.AI
{
    public class AiChatDto
    {
        public int chat_id { get; set; }
        public int teacher_id { get; set; }
        public string message { get; set; } = string.Empty;
        public string? chat_summary { get; set; }
        public DateTime created_at { get; set; }
    }
}