using System;
using System.Collections.Generic;

namespace demotienganh.Models;

public partial class Account
{
    public string Name { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Role { get; set; }
}
