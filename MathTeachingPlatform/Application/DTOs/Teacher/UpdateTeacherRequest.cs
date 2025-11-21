using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Teacher
{
    public class UpdateTeacherRequest
    {
        [StringLength(255, MinimumLength = 2)]
        public string? Name { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        public DateTime? HireDate { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }

        public TeacherStatus? Status { get; set; }
    }
}