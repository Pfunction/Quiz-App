
namespace QuizApp.Models
{
    public class Highscore
    {
        public int Id { get; set; }
<<<<<<< Updated upstream
        public string Email { get; set; }
=======
        public required string Email { get; set; }
>>>>>>> Stashed changes
        public int Score { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow; 
    }
}
