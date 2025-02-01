namespace QuizApp.Models
{
    public class Quiz
    {
        public int Id { get; set; }
<<<<<<< Updated upstream
        public string Title { get; set; }
        public List<Question> Questions { get; set; }
=======
        public required string Title { get; set; }
        public List<Question>? Questions { get; set; }


        //nav property
        public ICollection<QuizSubmission>? Submissions { get; set; }
>>>>>>> Stashed changes
    }
}
