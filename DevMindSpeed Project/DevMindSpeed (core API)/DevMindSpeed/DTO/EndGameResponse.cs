using System.Numerics;
using System.Text.Json.Serialization;

namespace DevMindSpeed.DTO
{
    public class EndGameResponse
    {

        public string Name { get; set; } = string.Empty;
        public int Difficulty { get; set; }

        public string CurrentScore { get; set; } = string.Empty;

        public double? TotalTimeSpent { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public string? Message { get; set; }

        public BestScoreDto? BestScore { get; set; }

        public List<QuestionHistoryDto> History { get; set; } = new();
    }

    public class BestScoreDto
    {
        public string Question { get; set; } = string.Empty;
        public double Answer { get; set; }
        public double? TimeTaken { get; set; }
    }

    public class QuestionHistoryDto
    {
        public string Question { get; set; } = string.Empty;
        public double? Submitted { get; set; }
        public double Correct { get; set; }
        public double? TimeTaken { get; set; }
    }





}
