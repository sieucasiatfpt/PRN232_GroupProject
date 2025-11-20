using Application.DTOs.AI;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class AiConfigService : IAiConfigService
    {
        private readonly IAiUnitOfWork _uow;

        public AiConfigService(IAiUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<AiConfigDto> createAsync(CreateAiConfigRequest req)
        {
            var entity = new AIConfig
            {
                TeacherId = req.teacher_id,
                ConfigName = req.config_name,
                ModelName = req.model_name,
                Temperature = req.temperature,
                MaxTokens = req.max_tokens,
                SettingsJson = req.settings_json,
                IsActive = req.is_active,
                CreatedAt = DateTime.UtcNow
            };
            await _uow.AIConfigs.AddAsync(entity);
            await _uow.SaveChangesAsync();
            return Map(entity);
        }

        public async Task<AiConfigDto> updateAsync(int id, UpdateAiConfigRequest req)
        {
            var entity = await _uow.AIConfigs.FirstOrDefaultAsync(x => x.ConfigId == id) ?? throw new Exception("AI config not found");
            if (req.config_name != null) entity.ConfigName = req.config_name;
            if (req.model_name != null) entity.ModelName = req.model_name;
            if (req.temperature.HasValue) entity.Temperature = req.temperature;
            if (req.max_tokens.HasValue) entity.MaxTokens = req.max_tokens;
            if (req.settings_json != null) entity.SettingsJson = req.settings_json;
            if (req.is_active.HasValue) entity.IsActive = req.is_active.Value;
            _uow.AIConfigs.Update(entity);
            await _uow.SaveChangesAsync();
            return Map(entity);
        }

        public async Task<bool> deleteAsync(int id)
        {
            var entity = await _uow.AIConfigs.FirstOrDefaultAsync(x => x.ConfigId == id) ?? throw new Exception("AI config not found");
            _uow.AIConfigs.Remove(entity);
            await _uow.SaveChangesAsync();
            return true;
        }

        public async Task<AiConfigDto> getAsync(int id)
        {
            var entity = await _uow.AIConfigs.FirstOrDefaultAsync(x => x.ConfigId == id) ?? throw new Exception("AI config not found");
            return Map(entity);
        }

        public async Task<List<AiConfigDto>> listByTeacherAsync(int teacherId)
        {
            var items = await _uow.AIConfigs.FindAsync(x => x.TeacherId == teacherId);
            return items.Select(Map).ToList();
        }

        private static AiConfigDto Map(AIConfig e) => new AiConfigDto
        {
            config_id = e.ConfigId,
            teacher_id = e.TeacherId,
            config_name = e.ConfigName,
            model_name = e.ModelName,
            temperature = e.Temperature,
            max_tokens = e.MaxTokens,
            settings_json = e.SettingsJson,
            is_active = e.IsActive,
            created_at = e.CreatedAt
        };
    }
}