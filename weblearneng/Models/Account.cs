using System;
using System.Collections.Generic;

namespace weblearneng.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Role { get; set; }

    public string Pass { get; set; } = null!;

    public virtual ICollection<ExamHistory> ExamHistories { get; set; } = new List<ExamHistory>();

    public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
}
