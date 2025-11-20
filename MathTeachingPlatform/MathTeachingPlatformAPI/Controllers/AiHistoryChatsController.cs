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
            r = r.Where(x => x.chat_id > 0).ToList();
            return Ok(r);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> soft_delete(int id)
        {
            // repository-level soft delete: set is_deleted=true
            // fetch via unit of work
            return await Task.Run(async () =>
            {
                var uow = HttpContext.RequestServices.GetRequiredService<Application.Interfaces.Repositories.IAiUnitOfWork>();
                var chat = await uow.AIHistoryChats.FirstOrDefaultAsync(x => x.ChatId == id);
                if (chat == null) return NotFound();
                chat.IsDeleted = true;
                uow.AIHistoryChats.Update(chat);
                await uow.SaveChangesAsync();
                return NoContent();
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> get(int id)
        {
            var r = await _svc.getAsync(id);
            return Ok(r);
        }
    }
}