

namespace QuizApp.Models
{
    public class QuizSubmission
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string? Email { get; set; }
        public List<SubmissionAnswer> Answers { get; set; }
    }
}

namespace QuizApp.Models
{
    public class SubmissionAnswer
    {
        public int Id { get; set; }
        public int QuizSubmissionId { get; set; }
        public int QuestionId { get; set; }
        public List<int> AnswerIds { get; set; } = new List<int>(); 
        public string? Text { get; set; }
    }
}
