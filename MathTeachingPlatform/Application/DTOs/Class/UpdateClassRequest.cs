using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Class
{
    public class UpdateClassRequest
    {
        [StringLength(255, MinimumLength = 2)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Schedule { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? SubjectId { get; set; }

        public int? TeacherId { get; set; }
    }
}