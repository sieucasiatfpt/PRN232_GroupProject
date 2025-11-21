using Application.DTOs.Exam;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("exam-questions")]
    public class ExamQuestionsController : ControllerBase
    {
        private readonly IContentUnitOfWork _uow;
        public ExamQuestionsController(IContentUnitOfWork uow) { _uow = uow; }

        [HttpGet]
        public async Task<IActionResult> list([FromQuery(Name = "matrix_id")] int? matrixId)
        {
            var q = _uow.ExamQuestions.Query();
            if (matrixId.HasValue) q = q.Where(x => x.MatrixId == matrixId.Value);
            var items = await q.OrderBy(x => x.QuestionId).ToListAsync();
            var r = items.Select(x => new ExamQuestionDto
            {
                question_id = x.QuestionId,
                syllabus_id = x.SyllabusId,
                matrix_id = x.MatrixId,
                question_text = x.QuestionText,
                question_type = "mcq",
                options_json = x.OptionsJson,
                answers = x.Answers,
                marks = x.Marks,
                points = x.Points,
                image_url = x.ImageUrl
            }).ToList();
            return Ok(r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> get(int id)
        {
            var x = await _uow.ExamQuestions.FirstOrDefaultAsync(e => e.QuestionId == id);
            if (x == null) return NotFound();
            var r = new ExamQuestionDto
            {
                question_id = x.QuestionId,
                syllabus_id = x.SyllabusId,
                matrix_id = x.MatrixId,
                question_text = x.QuestionText,
                question_type = "mcq",
                options_json = x.OptionsJson,
                answers = x.Answers,
                marks = x.Marks,
                points = x.Points,
                image_url = x.ImageUrl
            };
            return Ok(r);
        }
    }
}