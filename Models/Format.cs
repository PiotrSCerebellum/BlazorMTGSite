using System;
using System.Collections.Generic;

namespace MTG_app.Models;

public partial class Format
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
