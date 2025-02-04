using System.Text.Json.Serialization;

namespace QuizApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Text { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QuestionType Type { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}