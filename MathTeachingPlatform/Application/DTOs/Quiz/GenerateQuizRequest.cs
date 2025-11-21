namespace Application.DTOs.Quiz
{
    public class GenerateQuizRequest
    {
        public string? topic { get; set; }
        public int? grade { get; set; }
        public string? difficulty { get; set; }
        public int count { get; set; } = 5;
        public string? type { get; set; } = "multiple_choice";
        public int? teacher_id { get; set; }
        public int? student_id { get; set; }
        public int? config_id { get; set; }
    }
}