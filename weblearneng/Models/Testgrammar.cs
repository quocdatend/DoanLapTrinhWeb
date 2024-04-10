using System;
using System.Collections.Generic;

namespace weblearneng.Models;

public partial class Testgrammar
{
    public string Name { get; set; } = null!;

    public string? ExOr { get; set; }

    public string? Ex1 { get; set; }

    public string? Ex2 { get; set; }

    public string? Ex3 { get; set; }

    public virtual Grammar NameNavigation { get; set; } = null!;
}
