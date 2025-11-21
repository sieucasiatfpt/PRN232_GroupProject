using Application.DTOs.Exam;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("exam-assignments")]
    public class ExamAssignmentsController : ControllerBase
    {
        private readonly IContentUnitOfWork _uow;
        public ExamAssignmentsController(IContentUnitOfWork uow) { _uow = uow; }

        [HttpPost("publish")]
        public async Task<IActionResult> publish([FromBody] PublishAssignmentRequest req)
        {
            var a = new Domain.Entities.ExamAssignment
            {
                MatrixId = req.matrix_id,
                ClassId = req.class_id,
                StartTime = req.start_time,
                EndTime = req.end_time,
                Status = "published",
                CreatedAt = DateTime.UtcNow
            };
            await _uow.ExamAssignments.AddAsync(a);
            await _uow.SaveChangesAsync();
            return Ok(new { assignment_id = a.AssignmentId, matrix_id = a.MatrixId, class_id = a.ClassId, status = a.Status, start_time = a.StartTime, end_time = a.EndTime });
        }

        [HttpGet("students/{studentId}")]
        public async Task<IActionResult> listForStudent(int studentId)
        {
            var q = from cs in _uow.ClassStudents.Query()
                    join a in _uow.ExamAssignments.Query() on cs.ClassId equals a.ClassId
                    join m in _uow.ExamMatrices.Query() on a.MatrixId equals m.MatrixId
                    where cs.StudentId == studentId && a.Status == "published"
                    select new StudentExamItemDto
                    {
                        assignment_id = a.AssignmentId,
                        matrix_id = m.MatrixId,
                        class_id = cs.ClassId,
                        title = m.Title,
                        total_questions = m.TotalQuestions,
                        start_time = a.StartTime,
                        end_time = a.EndTime,
                        status = a.Status
                    };
            var items = await q.ToListAsync();
            return Ok(items);
        }
    }
}