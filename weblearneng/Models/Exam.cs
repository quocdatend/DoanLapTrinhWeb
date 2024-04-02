using System;
using System.Collections.Generic;

namespace weblearneng.Models;

public partial class Exam
{
    public int Examid { get; set; }

    public string? Examname { get; set; }

    public virtual ICollection<ExamHistory> ExamHistories { get; set; } = new List<ExamHistory>();

    public virtual ICollection<QuestionsContent> QuestionsContents { get; set; } = new List<QuestionsContent>();
}
