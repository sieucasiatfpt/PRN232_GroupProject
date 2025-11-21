using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Subject
{
    public class CreateSubjectRequest
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }
    }
}