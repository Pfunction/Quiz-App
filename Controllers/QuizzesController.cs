using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;

[ApiController]
[Route("api/[controller]")]
public class QuizzesController : ControllerBase
{
    private readonly QuizContext _context;

    public QuizzesController(QuizContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
    {
        return await _context.Quizzes
            .Include(q => q.Questions!)
            .ThenInclude(q => q.Answers!)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Quiz>> CreateQuiz(Quiz quiz)
    {
        if (quiz == null)
        {
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
        }

        int score = CalculateScore(quiz, submission);
        var highScore = new Highscore
        {
            Email = submission.Email ?? string.Empty,
            Score = score,
            DateTime = DateTime.Now
        };
        _context.HighScores.Add(highScore);
        await _context.SaveChangesAsync();

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
        {
            var submissionAnswer = submission.Answers!.FirstOrDefault(a => a.QuestionId == question.Id);
            if (submissionAnswer == null) continue;

            if (question.Type == "radio")
            {
                var selectedAnswerId = submissionAnswer.AnswerIds.FirstOrDefault();
                if (selectedAnswerId != 0){
                    var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == selectedAnswerId);
                
                if (selectedAnswer != null && selectedAnswer.IsCorrect)
                {
                    score += 100;
                }
                }
                
            }
            else if (question.Type == "checkbox")
            {
                var correctAnswers = question.Answers!.Where(a => a.IsCorrect).Select(a => a.Id).ToList();
                var selectedCorrectAnswers = submissionAnswer.AnswerIds!.Where(id => correctAnswers.Contains(id)).Count();
                score += (int)Math.Ceiling(100.0 / correctAnswers.Count * selectedCorrectAnswers);
            }
            else if (question.Type == "textbox")
            {
                var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect)?.Text.Trim().ToLower();
                var userAnswer = submissionAnswer.Text?.Trim().ToLower();
                if (correctAnswer == userAnswer)
                {
                    score += 100;
                }
               
            }
        }
        return score;
    }
    
}
