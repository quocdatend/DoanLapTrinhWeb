using System;
using System.Collections.Generic;

namespace weblearneng.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public bool Role { get; set; }

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();

    public virtual ICollection<Testgrammar> Testgrammars { get; set; } = new List<Testgrammar>();

    public virtual ICollection<Testvocabulary> Testvocabularies { get; set; } = new List<Testvocabulary>();

    public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
}
