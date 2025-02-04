namespace QuizApp.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public List<Question>? Questions { get; set; }


        //nav property
        public ICollection<QuizSubmission>? Submissions { get; set; }
    }
}
