using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Teacher
{
    public class TeacherDto
    {
        public int TeacherId { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public DateTime? HireDate { get; set; }
        public string? Department { get; set; }
        public TeacherStatus Status { get; set; }
        public List<SubjectInfoDto> Subjects { get; set; } = new();
        public List<ClassInfoDto> Classes { get; set; } = new();
    }

    public class TeacherIdDto
    {
        public int TeacherId { get; set; }
        public int UserId { get; set; }
    }

    public class SubjectInfoDto
    {
        public int SubjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class ClassInfoDto
    {
        public int ClassId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Schedule { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}