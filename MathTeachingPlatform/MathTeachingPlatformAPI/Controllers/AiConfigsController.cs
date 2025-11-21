using Application.DTOs.AI;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("ai-configs")]
    public class AiConfigsController : ControllerBase
    {
        private readonly IAiConfigService _svc;
        public AiConfigsController(IAiConfigService svc) { _svc = svc; }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateAiConfigRequest req)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var r = await _svc.createAsync(req);
            return Ok(r);
        }

        [HttpGet]
        public async Task<IActionResult> list([FromQuery(Name = "teacher_id")] int teacherId)
        {
            var r = await _svc.listByTeacherAsync(teacherId);
            return Ok(r);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> get(int id)
        {
            var r = await _svc.getAsync(id);
            return Ok(r);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id, [FromBody] UpdateAiConfigRequest req)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var r = await _svc.updateAsync(id, req);
            return Ok(r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            var ok = await _svc.deleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}