
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Services
{
    public class QuizService : IQuizService
    {
        private readonly QuizContext _context;

        public QuizService(QuizContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesAsync()
        {
            return await _context.Quizzes
                .Include(q => q.Questions!)
                .ThenInclude(q => q.Answers!)
                .ToListAsync();
        }

        public async Task<Quiz> CreateQuizAsync(Quiz quiz)
        {
            if (quiz == null)
            {
                throw new ArgumentNullException(nameof(quiz));
            }

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<int> SubmitQuizAsync(QuizSubmission submission)
        {
            if (submission == null)
            {
                throw new ArgumentNullException(nameof(submission));
            }

            var quiz = await _context.Quizzes
                .Include(q => q.Questions!)
                .ThenInclude(q => q.Answers!)
                .FirstOrDefaultAsync(q => q.Id == submission.QuizId);

            if (quiz == null)
            {
                throw new KeyNotFoundException("Quiz not found.");
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

        public async Task<IEnumerable<Highscore>> GetHighScoresAsync()
        {
            return await _context.HighScores
                .OrderByDescending(h => h.Score)
                .Take(10)
                .ToListAsync();
        }

        private int CalculateScore(Quiz quiz, QuizSubmission submission)
        {
            int score = 0;
            foreach (var question in quiz.Questions ?? Enumerable.Empty<Question>())
            {
                var submissionAnswer = submission.Answers!.FirstOrDefault(a => a.QuestionId == question.Id);
                if (submissionAnswer == null) continue;

                if (question.Type == QuestionType.Radio)
                {
                    var selectedAnswerId = submissionAnswer.AnswerIds?.FirstOrDefault() ?? 0;
                    if (selectedAnswerId != 0)
                    {
                        var selectedAnswer = question.Answers?.FirstOrDefault(a => a.Id == selectedAnswerId);
                        if (selectedAnswer != null && selectedAnswer.IsCorrect)
                        {
                            score += 100;
                        }
                    }
                }
                else if (question.Type == QuestionType.Checkbox)
                {
                    var correctAnswers = question.Answers!.Where(a => a.IsCorrect).Select(a => a.Id).ToList();
                    var selectedCorrectAnswers = submissionAnswer.AnswerIds!.Where(id => correctAnswers.Contains(id)).Count();
                    score += (int)Math.Ceiling(100.0 / correctAnswers.Count * selectedCorrectAnswers);
                }
                else if (question.Type == QuestionType.Textbox)
                {
                    var correctAnswer = question.Answers?.FirstOrDefault(a => a.IsCorrect)?.Text?.Trim().ToLower();
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
}