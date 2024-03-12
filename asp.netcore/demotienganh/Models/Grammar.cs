using System;
using System.Collections.Generic;

namespace demotienganh.Models;

public partial class Grammar
{
    public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Confirm { get; set; } = null!;

    public string Negative { get; set; } = null!;

    public string Doubt { get; set; } = null!;

    public virtual Testgrammar? Testgrammar { get; set; }
}
