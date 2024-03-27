using System;
using System.Collections.Generic;

namespace weblearneng.Models;

public partial class QuestionsContent
{
    public int Contentid { get; set; }

    public string? QuestionsStyle { get; set; }

    public string? Picture { get; set; }

    public string? TextContent { get; set; }

    public string? Adudi { get; set; }

    public string? TextQuestionsbigIfhave { get; set; }

    public int? Examid { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
