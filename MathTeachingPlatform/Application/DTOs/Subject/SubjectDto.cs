using Application.DTOs.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Subject
{
    public class SubjectDto
    {
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? TeacherName { get; set; }
        public List<ClassInfoDto> Classes { get; set; } = new();
    }
}