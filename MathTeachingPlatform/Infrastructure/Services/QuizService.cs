using Application.DTOs.Quiz;
using Application.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Services
{
    public class QuizService : IQuizService
    {
        private readonly AiDbContext _aiDb;

        public QuizService(AiDbContext aiDb)
        {
            _aiDb = aiDb;
        }

        public async Task<IReadOnlyList<QuizQuestionDto>> generateAsync(GenerateQuizRequest request, CancellationToken ct = default)
        {
            var topic = string.IsNullOrWhiteSpace(request.topic) ? "arithmetic" : request.topic.Trim().ToLowerInvariant();
            var diff = string.IsNullOrWhiteSpace(request.difficulty) ? "medium" : request.difficulty.Trim().ToLowerInvariant();
            var count = request.count <= 0 ? 5 : Math.Min(request.count, 50);

            var rand = new Random(unchecked(Environment.TickCount));
            var list = new List<QuizQuestionDto>(count);

            for (int i = 0; i < count; i++)
            {
                var q = MakeQuestion(rand, topic, diff, request.grade ?? 8);
                list.Add(q);
            }

            try
            {
                if (request.config_id.HasValue)
                {
                    var log = new Domain.Entities.AICallLog
                    {
                        ConfigId = request.config_id.Value,
                        StudentId = request.student_id,
                        ServiceName = "quiz.generate",
                        RequestData = System.Text.Json.JsonSerializer.Serialize(request),
                        ResponseData = System.Text.Json.JsonSerializer.Serialize(list),
                        CreatedAt = DateTime.UtcNow
                    };
                    _aiDb.AICallLogs.Add(log);
                    await _aiDb.SaveChangesAsync(ct);
                }
            }
            catch { }

            return list;
        }

        private static QuizQuestionDto MakeQuestion(Random rand, string topic, string difficulty, int grade)
        {
            int range = difficulty switch
            {
                "easy" => 10,
                "hard" => 100,
                _ => 50
            };

            if (topic == "algebra")
            {
                int a = rand.Next(1, range);
                int b = rand.Next(1, range);
                int x = rand.Next(1, range);
                int rhs = a * x + b;
                var question = $"Giải phương trình: {a}x + {b} = {rhs}";
                var correct = x;
                var choices = new[] { correct, correct + 1, correct - 1, correct + 2 };
                var shuffled = choices.Select(c => c.ToString()).OrderBy(_ => rand.Next()).ToArray();
                int ansIdx = Array.IndexOf(shuffled, correct.ToString());
                return new QuizQuestionDto
                {
                    id = Guid.NewGuid().ToString("n"),
                    question = question,
                    choices = shuffled,
                    answer_index = ansIdx,
                    explanation = "Đưa về dạng ax+b=c và giải x=(c-b)/a",
                    topic = topic,
                    difficulty = difficulty
                };
            }

            if (topic == "geometry")
            {
                int w = rand.Next(2, range);
                int h = rand.Next(2, range);
                int area = w * h;
                var question = $"Hình chữ nhật có chiều rộng {w} và chiều cao {h}. Diện tích bằng bao nhiêu?";
                var correct = area;
                var choices = new[] { correct, area + w, area - h, area + 2 };
                var shuffled = choices.Select(c => c.ToString()).OrderBy(_ => rand.Next()).ToArray();
                int ansIdx = Array.IndexOf(shuffled, correct.ToString());
                return new QuizQuestionDto
                {
                    id = Guid.NewGuid().ToString("n"),
                    question = question,
                    choices = shuffled,
                    answer_index = ansIdx,
                    explanation = "Diện tích hình chữ nhật = rộng × cao",
                    topic = topic,
                    difficulty = difficulty
                };
            }

            {
                int a = rand.Next(1, range);
                int b = rand.Next(1, range);
                var ops = new[] { "+", "-", "×", "÷" };
                var op = ops[rand.Next(ops.Length)];
                double correct = op switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "×" => a * b,
                    "÷" => Math.Round((double)a / Math.Max(1, b), 2),
                    _ => a + b
                };
                var question = $"Tính: {a} {op} {b}";
                var choices = new[]
                {
                    correct.ToString(),
                    (correct + 1).ToString(),
                    (correct - 1).ToString(),
                    (correct + 2).ToString()
                };
                var shuffled = choices.OrderBy(_ => rand.Next()).ToArray();
                int ansIdx = Array.IndexOf(shuffled, correct.ToString());
                return new QuizQuestionDto
                {
                    id = Guid.NewGuid().ToString("n"),
                    question = question,
                    choices = shuffled,
                    answer_index = ansIdx,
                    explanation = "Áp dụng phép toán tương ứng",
                    topic = topic,
                    difficulty = difficulty
                };
            }
        }
    }
}