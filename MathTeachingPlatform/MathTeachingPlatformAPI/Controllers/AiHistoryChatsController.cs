using Application.DTOs.AI;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("ai-history-chats")]
    public class AiHistoryChatsController : ControllerBase
    {
        private readonly IAiHistoryChatService _svc;
        public AiHistoryChatsController(IAiHistoryChatService svc) { _svc = svc; }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] CreateAiChatRequest req)
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
    }
}