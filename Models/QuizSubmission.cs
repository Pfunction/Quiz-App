using System.Collections.Generic;

public class QuizSubmission
{
    public int QuizId {get; set;}
    public string? Email {get; set;}
    public List<SubmissionAnswer>? Answers {get; set;}
}

public class SubmissionAnswer
{
    public int QuestionId {get; set;}
    public List<int>? AnswerIds {get;set;}
    public string? Text {get; set;}
}