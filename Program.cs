using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuizApp.Data;
using QuizApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddDbContext<QuizContext>(options =>
    options.UseInMemoryDatabase("QuizDb"));

builder.Services.AddScoped<IQuizService, QuizService>(); // Register the service

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quiz API", Version = "v1" });
});
builder.Services.AddLogging(configure => 
{
    configure.AddConsole();
    configure.AddDebug();
});

var app = builder.Build();

// DB
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<QuizContext>();
    context.Database.EnsureCreated(); 
}

//HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz API V1");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();