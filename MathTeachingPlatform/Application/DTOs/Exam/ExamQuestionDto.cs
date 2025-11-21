namespace Application.DTOs.Exam
{
    public class ExamQuestionDto
    {
        public int question_id { get; set; }
        public int? syllabus_id { get; set; }
        public int? matrix_id { get; set; }
        public string question_text { get; set; } = string.Empty;
        public string question_type { get; set; } = "mcq";
        public string? options_json { get; set; }
        public string answers { get; set; } = string.Empty;
        public int? marks { get; set; }
        public decimal? points { get; set; }
        public string? image_url { get; set; }
    }
}