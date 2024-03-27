using System;
using System.Collections.Generic;

namespace weblearneng.Models;

public partial class Exam
{
    public int Examid { get; set; }

    public string? Examname { get; set; }

    public virtual ICollection<QuestionsContent> QuestionsContents { get; set; } = new List<QuestionsContent>();

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();

    public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
}
