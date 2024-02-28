using MTG.Models;
using Microsoft.EntityFrameworkCore;

namespace MTG.Services
{
    public class Services
    {
        MyDBContext dbContext = new MyDBContext();
        public IQueryable GetRedCards()
        {
            IQueryable redCards = dbContext.CardColors.Include(c => c.Color)
                .Where(w => w.CardId >= 1000 && w.CardId < 1050 && w.Color.Name == "Red")
                .Select(p => new RedCardModel
                {
                    Id = p.CardId,
                    Name = p.Card.Name,
                    Image = p.Card.OriginalImageUrl,
                    Color = p.Color.Name
                })
                .OrderBy(o => o.Name);
            return redCards;
        }
        public class RedCardModel
        {
            public long? Id { get; init; }
            public string? Name { get; init; }
            public string? Image { get; init; }
            public string? Color { get; init; }

        }



    }
}
