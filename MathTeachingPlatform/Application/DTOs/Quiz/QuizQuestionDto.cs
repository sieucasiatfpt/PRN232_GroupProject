namespace Application.DTOs.Quiz
{
    public class QuizQuestionDto
    {
        public string id { get; set; } = string.Empty;
        public string question { get; set; } = string.Empty;
        public string[] choices { get; set; } = Array.Empty<string>();
        public int answer_index { get; set; }
        public string? explanation { get; set; }
        public string? topic { get; set; }
        public string? difficulty { get; set; }
    }
}