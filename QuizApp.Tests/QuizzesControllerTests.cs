using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using QuizApp.Data;
using QuizApp.Models;


namespace QuizApp.Tests
{
    public class QuizzesControllerTests
    {
        private QuizContext CreateTestContext()
        {
            var options = new DbContextOptionsBuilder<QuizContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            var context = new QuizContext(options);
            
   
            var quiz = new Quiz 
            { 
                Id = 1, 
                Title = "Test Quiz",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Id = 1,
                        QuizId = 1,
                        Text = "Test Question 1",
                        Type = "radio",
                        Answers = new List<Answer>
                        {
                            new Answer { Id = 1, QuestionId = 1, Text = "Correct", IsCorrect = true },
                            new Answer { Id = 2, QuestionId = 1, Text = "Wrong", IsCorrect = false }
                        }
                    },
                    new Question
                    {
                        Id = 2,
                        QuizId = 1,
                        Text = "Test Question 2",
                        Type = "checkbox",
                        Answers = new List<Answer>
                        {
                            new Answer { Id = 3, QuestionId = 2, Text = "Correct 1", IsCorrect = true },
                            new Answer { Id = 4, QuestionId = 2, Text = "Correct 2", IsCorrect = true },
                            new Answer { Id = 5, QuestionId = 2, Text = "Wrong", IsCorrect = false }
                        }
                    }
                }
            };

            context.Quizzes.Add(quiz);
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetQuizzes_ReturnsAllQuizzes()
        {
          
            using var context = CreateTestContext();
            var controller = new QuizzesController(context);

      
            var result = await controller.GetQuizzes();

       
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Quiz>>>(result);
            var quizzes = Assert.IsAssignableFrom<IEnumerable<Quiz>>(actionResult.Value);
            Assert.Single(quizzes);
            Assert.Equal("Test Quiz", quizzes.First().Title);
            Assert.Equal(2, quizzes.First().Questions.Count);
        }

        [Fact]
        public async Task CreateQuiz_ValidQuiz_ReturnsCreatedResponse()
        {
           
            using var context = CreateTestContext();
            var controller = new QuizzesController(context);
            var newQuiz = new Quiz { Title = "New Quiz" };

         
            var result = await controller.CreateQuiz(newQuiz);

         
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedQuiz = Assert.IsType<Quiz>(actionResult.Value);
            Assert.Equal("New Quiz", returnedQuiz.Title);
        }

        [Fact]
        public async Task SubmitQuiz_CorrectAnswers_ReturnsFullScore()
        {
         
            using var context = CreateTestContext();
            var controller = new QuizzesController(context);
            var submission = new QuizSubmission
            {
                QuizId = 1,
                Email = "test@example.com",
                Answers = new List<SubmissionAnswer>
                {
                    new SubmissionAnswer
                    {
                        QuestionId = 1,
                        AnswerIds = new List<int> { 1 }  // Correct radio answer
                    },
                    new SubmissionAnswer
                    {
                        QuestionId = 2,
                        AnswerIds = new List<int> { 3, 4 }  // Both correct checkbox answers
                    }
                }
            };

        
            var result = await controller.SubmitQuiz(submission);

   
            var actionResult = Assert.IsType<ActionResult<int>>(result);
            var score = Assert.IsType<int>(actionResult.Value);
            Assert.Equal(200, score); // 100 points per question
        }

        [Fact]
        public async Task GetHighScores_ReturnsTopScores()
        {
        
            using var context = CreateTestContext();
            var controller = new QuizzesController(context);
            
       
            context.HighScores.AddRange(
                new Highscore { Email = "test1@example.com", Score = 100 },
                new Highscore { Email = "test2@example.com", Score = 200 },
                new Highscore { Email = "test3@example.com", Score = 150 }
            );
            await context.SaveChangesAsync();

         
            var result = await controller.GetHighScores();


            var actionResult = Assert.IsType<ActionResult<IEnumerable<Highscore>>>(result);
            var highscores = Assert.IsAssignableFrom<IEnumerable<Highscore>>(actionResult.Value);
            Assert.Equal(3, highscores.Count());
            Assert.Equal(200, highscores.First().Score);  
        }
    }
}