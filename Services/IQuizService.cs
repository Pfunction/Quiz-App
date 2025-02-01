
using QuizApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizApp.Services
{
    public interface IQuizService
    {
        Task<IEnumerable<Quiz>> GetQuizzesAsync();
        Task<Quiz> CreateQuizAsync(Quiz quiz);
        Task<int> SubmitQuizAsync(QuizSubmission submission);
        Task<IEnumerable<Highscore>> GetHighScoresAsync();
    }
}