using System.Text.Json.Serialization;

namespace DevMindSpeed.DTO
{
 
        public class SubmitAnswerResponse
        {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public string? Result { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public double? TimeTaken { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public NextQuestionDto? NextQuestion { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public string? CurrentScore { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public string? Message { get; set; }
    
    }

        public class NextQuestionDto
        {
            public string SubmitUrl { get; set; } = string.Empty;
            public string Question { get; set; } = string.Empty;
        }


    
}
