using Application.DTOs.AI;

namespace Application.Interfaces
{
    public interface IAiHistoryChatService
    {
        Task<AiChatDto> createAsync(CreateAiChatRequest req);
        Task<AiChatDto> getAsync(int id);
        Task<List<AiChatDto>> listByTeacherAsync(int teacherId);
    }
}