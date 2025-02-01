

namespace QuizApp.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
<<<<<<< Updated upstream
        public string Text { get; set; }
=======
        public required string Text { get; set; }
>>>>>>> Stashed changes
        public bool IsCorrect { get; set; }
    }
}
