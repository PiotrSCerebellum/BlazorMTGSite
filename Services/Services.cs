using MTG.Models;
using Microsoft.EntityFrameworkCore;

namespace MTG.Services
{
    public class Services
    {
        MyDBContext dbContext = new MyDBContext();
        public List<string> GetColors()
        {
            List<string> colors = dbContext.Colors.Select(p => p.Name).ToList();

            return colors;
        }
        public List<string> GetTypes()
        {
            List<string> types = dbContext.Types.Select(p => p.Name).ToList();

            return types;
        }
        public List<string> GetRarities()
        {
            List<string> rarities = dbContext.Rarities.Select(p => p.Name).ToList();

            return rarities;
        }

        public IQueryable GetCards(Search search)
        {
            IQueryable cards = dbContext.Cards.Include(c => c.CardColors).Include(c => c.CardTypes)
                .Where(w => w.Name.Contains(search.cardName) && w.Text.Contains(search.cardText) && w.Cmc == search.cmc)
                .Select(p => new CardModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Image = p.OriginalImageUrl
                })
                .OrderBy(o => o.Name);
            return cards;
        }

        public IQueryable GetCardsByColor(string color,int idRange)
        {
            IQueryable cardsByColor = dbContext.CardColors.Include(c => c.Color)
                .Where(w => w.CardId >= idRange && w.CardId < idRange+50 && w.Color.Name == color)
                .Select(p => new CardModel
                {
                    Id = p.CardId,
                    Name = p.Card.Name,
                    Image = p.Card.OriginalImageUrl,
                    Color = p.Color.Name
                })
                .OrderBy(o => o.Name);
            return cardsByColor;
        }
        public class CardModel
        {
            public long? Id { get; init; }
            public string? Name { get; init; }
            public string? Image { get; init; }
            public string? Color { get; init; }

        }

        public IQueryable GetCardsByType(string type, int idRange)
        {
            IQueryable cardsByType = dbContext.CardTypes.Include(c => c.Type)
                .Where(w => w.CardId >= idRange && w.CardId < idRange + 50 && w.Type.Name == type)
                .Select(p => new CardModel
                {
                    Id = p.CardId,
                    Name = p.Card.Name,
                    Image = p.Card.OriginalImageUrl
                })
                .OrderBy(o => o.Name);
            return cardsByType;
        }




    }
}
