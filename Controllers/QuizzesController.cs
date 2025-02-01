
using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizzesController(IQuizService quizService)
        {
<<<<<<< Updated upstream
            return BadRequest();
        }
        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetQuizzes), new { id = quiz.Id }, quiz);
    }

    [HttpPost("submit")]
    public async Task<ActionResult<int>> SubmitQuiz([FromBody] QuizSubmission submission)
    {
        if (submission == null)
        {
            return BadRequest();
        }
        Console.WriteLine($"Quiz Submission: {submission.QuizId}, User: {submission.UserId}, Email: {submission.Email}");
    foreach (var answer in submission.Answers)
    {
        Console.WriteLine($"Question ID: {answer.QuestionId}, Answer IDs: {string.Join(", ", answer.AnswerIds)}");
    }        
        var quiz = await _context.Quizzes
            .Include(q => q.Questions!)
            .ThenInclude(q => q.Answers!)
            .FirstOrDefaultAsync(q => q.Id == submission.QuizId);

        if (quiz == null)
        {
            return NotFound();
=======
            _quizService = quizService ?? throw new ArgumentNullException(nameof(quizService));
>>>>>>> Stashed changes
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
        {
            var quizzes = await _quizService.GetQuizzesAsync();
            return Ok(quizzes);
        }

<<<<<<< Updated upstream
        return score;
    }

    [HttpGet("highscores")]
    public async Task<ActionResult<IEnumerable<Highscore>>> GetHighScores()
    {
        var highScores = await _context.HighScores
            .OrderByDescending(h => h.Score)
            .Take(10)
            .ToListAsync();
        return highScores;
    }

    private int CalculateScore(Quiz quiz, QuizSubmission submission)
    {
        int score = 0;
        foreach (var question in quiz.Questions)
=======
        [HttpPost]
        public async Task<ActionResult<Quiz>> CreateQuiz(Quiz quiz)
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
                var selectedAnswerId = submissionAnswer.AnswerIds.FirstOrDefault();
                if (selectedAnswerId != 0){
                    var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == selectedAnswerId);
                
                if (selectedAnswer != null && selectedAnswer.IsCorrect)
                {
                    score += 100;
                }
                }
                
=======
                return BadRequest();
>>>>>>> Stashed changes
            }

            try
            {
                var score = await _quizService.SubmitQuizAsync(submission);
                return Ok(score);
            }
            catch (KeyNotFoundException ex)
            {
<<<<<<< Updated upstream
                var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect)?.Text.Trim().ToLower();
                var userAnswer = submissionAnswer.Text?.Trim().ToLower();
                if (correctAnswer == userAnswer)
                {
                    score += 100;
                }
               
=======
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
>>>>>>> Stashed changes
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