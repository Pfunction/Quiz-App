
namespace QuizApp.Models
{
    public class SubmissionAnswer
    {
        public int Id { get; set; }
        public int QuizSubmissionId { get; set; }
        public int QuestionId { get; set; }
        public List<int> AnswerIds { get; set; } = new List<int>(); 
        public string? Text { get; set; }

        //nav properties
         public QuizSubmission? QuizSubmission { get; set; }
        public Question? Question { get; set; }
    }
}