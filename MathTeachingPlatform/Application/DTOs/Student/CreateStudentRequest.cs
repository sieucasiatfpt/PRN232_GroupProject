using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Student
{
    public class CreateStudentRequest
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        public string? Major { get; set; }

        public int? ClassId { get; set; }

        public DateTime? EnrollmentDate { get; set; }
    }
}
