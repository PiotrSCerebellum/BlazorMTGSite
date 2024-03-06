using System;
using System.Collections.Generic;

namespace MTG.Models;

public partial class Color
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CardColor> CardColors { get; set; } = new List<CardColor>();
}
