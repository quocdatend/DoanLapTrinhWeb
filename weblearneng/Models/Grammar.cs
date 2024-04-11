using System;
using System.Collections.Generic;

namespace weblearneng.Models;

public partial class Grammar
{
    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Confirm { get; set; }

    public string? Doubt { get; set; }

    public string? Negative { get; set; }

    public string? Order { get; set; }

    public virtual ICollection<Testgrammar> Testgrammars { get; set; } = new List<Testgrammar>();
}
