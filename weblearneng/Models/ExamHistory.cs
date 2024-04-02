using System;
using System.Collections.Generic;

namespace weblearneng.Models;

public partial class ExamHistory
{
    public int ScoreId { get; set; }

    public int UserId { get; set; }

    public int ExamId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public double Score { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual Account User { get; set; } = null!;
}
