
using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.Services;


namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizzesController(IQuizService quizService)
        {
            _quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
        {
            var quizzes = await _quizService.GetQuizzesAsync();
            return Ok(quizzes);
        }

        [HttpPost]
        public async Task<ActionResult<Quiz>> CreateQuiz(Quiz quiz)
        {
            if (quiz == null)
            {
                return BadRequest();
            }

            var createdQuiz = await _quizService.CreateQuizAsync(quiz);
            return CreatedAtAction(nameof(GetQuizzes), new { id = createdQuiz.Id }, createdQuiz);
        }

        [HttpPost("submit")]
        public async Task<ActionResult<int>> SubmitQuiz([FromBody] QuizSubmission submission)
        {
            if (submission == null)
            {
                return BadRequest();
            }

            try
            {
                var score = await _quizService.SubmitQuizAsync(submission);
                return Ok(score);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("highscores")]
        public async Task<ActionResult<IEnumerable<Highscore>>> GetHighScores()
        {
            var highScores = await _quizService.GetHighScoresAsync();
            return Ok(highScores);
        }
    }
}