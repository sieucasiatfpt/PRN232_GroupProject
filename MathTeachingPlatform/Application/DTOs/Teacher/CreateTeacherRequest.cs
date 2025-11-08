using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Teacher
{
    public class CreateTeacherRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Bio { get; set; }

        public DateTime? HireDate { get; set; }

        [StringLength(100)]
        public string? Department { get; set; }
    }
}