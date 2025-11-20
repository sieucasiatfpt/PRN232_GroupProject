using Application.DTOs.AI;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("ai-call-logs")]
    public class AiCallLogsController : ControllerBase
    {
        private readonly IAiUnitOfWork _uow;
        public AiCallLogsController(IAiUnitOfWork uow) { _uow = uow; }

        [HttpGet]
        public async Task<IActionResult> list([FromQuery(Name = "config_id")] int? configId, [FromQuery(Name = "student_id")] int? studentId)
        {
            var logs = await _uow.AICallLogs.GetAllAsync();
            var q = logs.AsQueryable();
            if (configId.HasValue) q = q.Where(x => x.ConfigId == configId.Value).ToList().AsQueryable();
            if (studentId.HasValue) q = q.Where(x => x.StudentId == studentId.Value).ToList().AsQueryable();
            var r = q.OrderByDescending(x => x.CreatedAt).Select(x => new AiCallLogDto
            {
                log_id = x.LogId,
                config_id = x.ConfigId,
                student_id = x.StudentId,
                matrix_id = x.MatrixId,
                service_name = x.ServiceName,
                request_text = x.RequestText,
                response_text = x.ResponseText,
                created_at = x.CreatedAt
            }).ToList();
            return Ok(r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> get(int id)
        {
            var x = await _uow.AICallLogs.FirstOrDefaultAsync(l => l.LogId == id);
            if (x == null) return NotFound();
            var r = new AiCallLogDto
            {
                log_id = x.LogId,
                config_id = x.ConfigId,
                student_id = x.StudentId,
                matrix_id = x.MatrixId,
                service_name = x.ServiceName,
                request_text = x.RequestText,
                response_text = x.ResponseText,
                created_at = x.CreatedAt
            };
            return Ok(r);
        }
    }
}