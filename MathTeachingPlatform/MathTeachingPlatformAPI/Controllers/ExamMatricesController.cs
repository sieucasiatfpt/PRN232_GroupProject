using Application.DTOs.Exam;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("exam-matrices")]
    public class ExamMatricesController : ControllerBase
    {
        private readonly IContentUnitOfWork _uow;
        public ExamMatricesController(IContentUnitOfWork uow) { _uow = uow; }

        [HttpGet]
        public async Task<IActionResult> list([FromQuery(Name = "subject_id")] int? subjectId)
        {
            var q = _uow.ExamMatrices.Query();
            if (subjectId.HasValue) q = q.Where(x => x.SubjectId == subjectId.Value);
            var items = await q.OrderByDescending(x => x.GeneratedOn).ToListAsync();
            var r = items.Select(x => new ExamMatrixDto
            {
                matrix_id = x.MatrixId,
                subject_id = x.SubjectId,
                title = x.Title,
                difficulty_distribution = x.DifficultyDistribution,
                total_questions = x.TotalQuestions,
                generated_on = x.GeneratedOn
            }).ToList();
            return Ok(r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> get(int id)
        {
            var x = await _uow.ExamMatrices.FirstOrDefaultAsync(e => e.MatrixId == id);
            if (x == null) return NotFound();
            var r = new ExamMatrixDto
            {
                matrix_id = x.MatrixId,
                subject_id = x.SubjectId,
                title = x.Title,
                difficulty_distribution = x.DifficultyDistribution,
                total_questions = x.TotalQuestions,
                generated_on = x.GeneratedOn
            };
            return Ok(r);
        }
    }
}