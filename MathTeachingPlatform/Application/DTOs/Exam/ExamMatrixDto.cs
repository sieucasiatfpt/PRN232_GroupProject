namespace Application.DTOs.Exam
{
    public class ExamMatrixDto
    {
        public int matrix_id { get; set; }
        public int subject_id { get; set; }
        public string title { get; set; } = string.Empty;
        public string? difficulty_distribution { get; set; }
        public int total_questions { get; set; }
        public DateTime generated_on { get; set; }
    }
}