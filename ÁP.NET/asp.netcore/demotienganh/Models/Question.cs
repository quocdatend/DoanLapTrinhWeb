using System;
using System.Collections.Generic;

namespace demotienganh.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public int Qcid { get; set; }

    public string? QuestionText { get; set; }

    public string? AnswerA { get; set; }

    public string? AnswerB { get; set; }

    public string? AnswerC { get; set; }

    public string? AnswerD { get; set; }

    public string? CorrectAnswer { get; set; }

    public virtual QuestionsContent Qc { get; set; } = null!;

    public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
}
