
using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions<QuizContext> options) :base(options) {}


        //DBset
        public DbSet<Quiz> Quizzes {get; set;}
        public DbSet<Question> Questions {get; set;}
        public DbSet<Answer> Answers {get; set;}

        public DbSet<Highscore> HighScores {get;set;}
    }
}