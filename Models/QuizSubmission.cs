namespace QuizApp.Models
{
    public class QuizSubmission
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int UserId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public required string Email { get; set; }
        public List<SubmissionAnswer>? Answers { get; set; }


        
        //nav property
        public Quiz? Quiz { get; set; }
    }
}

