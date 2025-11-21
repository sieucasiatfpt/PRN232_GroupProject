using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Student
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Major { get; set; }
        public List<ClassInfoDto> EnrolledClasses { get; set; } = new();
        public DateTime? EnrollmentDate { get; set; }
        public StudentStatus Status { get; set; }
    }
}
