using System;
using System.Collections.Generic;

namespace demotienganh.Models;

public partial class Vocabulary
{
    public string Firstchar { get; set; } = null!;

    public string NameEn { get; set; } = null!;

    public string NameVn { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Title { get; set; }
}
