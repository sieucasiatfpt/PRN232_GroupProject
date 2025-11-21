using Application.DTOs.AI;

namespace Application.Interfaces
{
    public interface IAiConfigService
    {
        Task<AiConfigDto> createAsync(CreateAiConfigRequest req);
        Task<AiConfigDto> updateAsync(int id, UpdateAiConfigRequest req);
        Task<bool> deleteAsync(int id);
        Task<AiConfigDto> getAsync(int id);
        Task<List<AiConfigDto>> listByTeacherAsync(int teacherId);
    }
}