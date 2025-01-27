using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions<QuizContext> options) : base(options) { }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Highscore> HighScores { get; set; }
        public DbSet<QuizSubmission> QuizSubmissions { get; set; }
        public DbSet<SubmissionAnswer> SubmissionAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quiz>().HasData(
                new Quiz { Id = 1, Title = "Quiz Master" }
            );

            modelBuilder.Entity<Question>().HasData(
            new Question { Id = 1, QuizId = 1, Text = "What is 5 + 7?", Type = "radio" },
            new Question { Id = 2, QuizId = 1, Text = "What is the capital city of France?", Type = "textbox" },
            new Question { Id = 3, QuizId = 1, Text = "What is the result of 9 x 6?", Type = "radio" },
            new Question { Id = 4, QuizId = 1, Text = "What is the longest river in the world?", Type = "radio" },
            new Question { Id = 5, QuizId = 1, Text = "What is the square root of 144?", Type = "radio" },
            new Question { Id = 6, QuizId = 1, Text = "Which of the following are continents? (Select all that apply)", Type = "checkbox" },
            new Question { Id = 7, QuizId = 1, Text = "What is the value of π (Pi) to 3 decimal places?", Type = "textbox" },
            new Question { Id = 8, QuizId = 1, Text = "Who invented the telephone?", Type = "radio" },
            new Question { Id = 9, QuizId = 1, Text = "What is the sum of the angles in a triangle? (Type only the number)", Type = "textbox" },
            new Question { Id = 10, QuizId = 1, Text = "Which country has the most official languages?", Type = "checkbox" }
        );

        modelBuilder.Entity<Answer>().HasData(
            // Question 1
            new Answer { Id = 1, QuestionId = 1, Text = "10", IsCorrect = false },
            new Answer { Id = 2, QuestionId = 1, Text = "11", IsCorrect = false },
            new Answer { Id = 3, QuestionId = 1, Text = "12", IsCorrect = true },
            new Answer { Id = 4, QuestionId = 1, Text = "13", IsCorrect = false },
            // Question 2
            new Answer { Id = 5, QuestionId = 2, Text = "Paris", IsCorrect = true },
            // Question 3
            new Answer { Id = 6, QuestionId = 3, Text = "48", IsCorrect = false },
            new Answer { Id = 7, QuestionId = 3, Text = "54", IsCorrect = true },
            new Answer { Id = 8, QuestionId = 3, Text = "56", IsCorrect = false },
            new Answer { Id = 9, QuestionId = 3, Text = "60", IsCorrect = false },
            // Question 4
            new Answer { Id = 10, QuestionId = 4, Text = "Amazon", IsCorrect = false },
            new Answer { Id = 11, QuestionId = 4, Text = "Nile", IsCorrect = true },
            new Answer { Id = 12, QuestionId = 4, Text = "Mississippi", IsCorrect = false },
            new Answer { Id = 13, QuestionId = 4, Text = "Yangtze", IsCorrect = false },
            // Question 5
            new Answer { Id = 14, QuestionId = 5, Text = "10", IsCorrect = false },
            new Answer { Id = 15, QuestionId = 5, Text = "11", IsCorrect = false },
            new Answer { Id = 16, QuestionId = 5, Text = "12", IsCorrect = true },
            new Answer { Id = 17, QuestionId = 5, Text = "14", IsCorrect = false },
            // Question 6
            new Answer { Id = 18, QuestionId = 6, Text = "Africa", IsCorrect = true },
            new Answer { Id = 19, QuestionId = 6, Text = "Asia", IsCorrect = true },
            new Answer { Id = 20, QuestionId = 6, Text = "Antarctica", IsCorrect = true },
            new Answer { Id = 21, QuestionId = 6, Text = "Greenland", IsCorrect = false },
            // question 7
            new Answer { Id = 22, QuestionId = 7, Text = "3.142", IsCorrect = true },
            // question 8
            new Answer { Id = 23, QuestionId = 8, Text = "Albert Einstein", IsCorrect = false },
            new Answer { Id = 24, QuestionId = 8, Text = "Nikola Tesla", IsCorrect = false },
            new Answer { Id = 25, QuestionId = 8, Text = "Alexander Graham Bell", IsCorrect = true },
            new Answer { Id = 26, QuestionId = 8, Text = "Thomas Edison", IsCorrect = false },
            // question 9
            new Answer { Id = 27, QuestionId = 9, Text = "180", IsCorrect = true },
            // Question 10
            new Answer { Id = 28, QuestionId = 10, Text = "India", IsCorrect = false },
            new Answer { Id = 29, QuestionId = 10, Text = "Switzerland", IsCorrect = false },
            new Answer { Id = 30, QuestionId = 10, Text = "South Africa", IsCorrect = true },
            new Answer { Id = 31, QuestionId = 10, Text = "Canada", IsCorrect = false }
        );


            modelBuilder.Entity<QuizSubmission>().HasData(
                new QuizSubmission { Id = 1, QuizId = 1, UserId = 1, SubmissionDate = DateTime.Now, Email = "test@example.com" }
            );

            modelBuilder.Entity<SubmissionAnswer>().HasData(
                new SubmissionAnswer { Id = 1, QuizSubmissionId = 1, QuestionId = 1, AnswerIds = new List<int> { 2 } }
            );
        }
    }
}
