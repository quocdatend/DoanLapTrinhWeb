using System;
using System.Collections.Generic;

namespace weblearneng.Models;

public partial class Score
{
    public int ScoreId { get; set; }

    public int? UserId { get; set; }

    public int? ExamId { get; set; }

    public int? TotalScore { get; set; }

    public virtual Exam? Exam { get; set; }

    public virtual Account? User { get; set; }
}
