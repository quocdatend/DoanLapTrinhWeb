using System;
using System.Collections.Generic;

namespace weblearneng.Models;

public partial class Testvocabulary
{
    public int Id { get; set; }

    public string Nameen { get; set; } = null!;

    public string Correct { get; set; } = null!;

    public string Fail1 { get; set; } = null!;

    public string Fail2 { get; set; } = null!;

    public string Fail3 { get; set; } = null!;

    public virtual Account IdNavigation { get; set; } = null!;

    public virtual Vocabulary NameenNavigation { get; set; } = null!;
}
