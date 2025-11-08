using Application.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Class
{
    public class ClassDto
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Schedule { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? SubjectTitle { get; set; }
        public string? TeacherName { get; set; }
        public List<StudentInfoDto> EnrolledStudents { get; set; } = new();
    }

    public class StudentInfoDto
    {
        public int StudentId { get; set; }
        public string? Name { get; set; }
        public string EnrollmentStatus { get; set; } = string.Empty;
        public DateTime EnrolledAt { get; set; }
    }
}