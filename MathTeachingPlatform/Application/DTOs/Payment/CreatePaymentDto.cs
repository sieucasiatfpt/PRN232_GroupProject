namespace Application.DTOs.Payment
{
    public class CreatePaymentDto
    {
        public int TeacherId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string TeacherName { get; set; } = string.Empty; 
    }
}