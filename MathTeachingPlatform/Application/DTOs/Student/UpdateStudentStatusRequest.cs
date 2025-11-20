using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Student
{
    public class UpdateStudentStatusRequest
    {
        [Required]
        public StudentStatus Status { get; set; }

        public string? Reason { get; set; }
    }
}
