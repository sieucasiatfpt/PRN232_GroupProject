namespace Application.DTOs.Exam
{
    public class StudentExamItemDto
    {
        public int assignment_id { get; set; }
        public int matrix_id { get; set; }
        public int class_id { get; set; }
        public string title { get; set; } = string.Empty;
        public int total_questions { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public string status { get; set; } = string.Empty;
    }
}