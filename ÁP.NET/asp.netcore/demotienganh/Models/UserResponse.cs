using System;
using System.Collections.Generic;

namespace demotienganh.Models;

public partial class UserResponse
{
    public int ResponseId { get; set; }

    public int? UserId { get; set; }

    public int? ExamId { get; set; }

    public int? QuestionId { get; set; }

    public string? UserAnswer { get; set; }

    public int? IsCorrect { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual Question? Question { get; set; }

    public virtual Account? User { get; set; }
}
