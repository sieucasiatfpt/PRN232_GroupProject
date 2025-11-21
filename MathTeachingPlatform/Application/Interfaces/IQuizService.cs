using Application.DTOs.Quiz;

namespace Application.Interfaces
{
    public interface IQuizService
    {
        Task<IReadOnlyList<QuizQuestionDto>> generateAsync(GenerateQuizRequest request, CancellationToken ct = default);
    }
}