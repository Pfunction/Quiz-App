
namespace QuizApp.Models
{
    public class Highscore
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public int Score { get; set; }
        public DateTime DateTime { get; set; }
    }
}
