using Application.DTOs.AI;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class AiHistoryChatService : IAiHistoryChatService
    {
        private readonly IAiUnitOfWork _uow;

        public AiHistoryChatService(IAiUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<AiChatDto> createAsync(CreateAiChatRequest req)
        {
            var entity = new AIHistoryChat
            {
                TeacherId = req.teacher_id,
                Message = req.message,
                ChatSummary = req.chat_summary,
                CreatedAt = DateTime.UtcNow
            };
            await _uow.AIHistoryChats.AddAsync(entity);
            await _uow.SaveChangesAsync();
            return Map(entity);
        }

        public async Task<AiChatDto> getAsync(int id)
        {
            var entity = await _uow.AIHistoryChats.FirstOrDefaultAsync(x => x.ChatId == id) ?? throw new Exception("Chat not found");
            return Map(entity);
        }

        public async Task<List<AiChatDto>> listByTeacherAsync(int teacherId)
        {
            var items = await _uow.AIHistoryChats.FindAsync(x => x.TeacherId == teacherId);
            return items.OrderByDescending(x => x.CreatedAt).Select(Map).ToList();
        }

        private static AiChatDto Map(AIHistoryChat e) => new AiChatDto
        {
            chat_id = e.ChatId,
            teacher_id = e.TeacherId,
            message = e.Message,
            chat_summary = e.ChatSummary,
            created_at = e.CreatedAt
        };
    }
}