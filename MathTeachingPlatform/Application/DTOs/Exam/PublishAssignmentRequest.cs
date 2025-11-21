namespace Application.DTOs.Exam
{
    public class PublishAssignmentRequest
    {
        public int matrix_id { get; set; }
        public int class_id { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
    }
}