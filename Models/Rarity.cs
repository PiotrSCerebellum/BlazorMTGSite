using System;
using System.Collections.Generic;

namespace MTG_app.Models;

public partial class Rarity
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
