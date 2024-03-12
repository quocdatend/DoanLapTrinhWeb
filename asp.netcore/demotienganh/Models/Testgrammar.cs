using System;
using System.Collections.Generic;

namespace demotienganh.Models;

public partial class Testgrammar
{
    public string Name { get; set; } = null!;

    public string Question { get; set; } = null!;

    public string Correct { get; set; } = null!;

    public string Fail1 { get; set; } = null!;

    public string Fail2 { get; set; } = null!;

    public string Fail3 { get; set; } = null!;

    public virtual Grammar NameNavigation { get; set; } = null!;
}
