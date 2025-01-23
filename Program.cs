using Microsoft.EntityFrameworkCore;
using QuizApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QuizContext>(options =>
    options.UseInMemoryDatabase("QuizDb"));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
