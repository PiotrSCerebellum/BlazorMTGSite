using MTG.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MTG.Services
{
    public class Search
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
        public IQueryable<SimpleCardModel> GetCollection(string username)
        {     
            string collection = dbContext.Users
                .Where(w => w.Username == username)
                .Select(p => p.Collection)
                .FirstOrDefault();
            Console.WriteLine("Getting collection of" + username + collection);
            string[] subs = collection.Split(',');

            var cardIds = subs.Select(int.Parse).ToList(); 
            IQueryable<Card> cards = dbContext.Cards.Where(w => cardIds.Contains((int)w.Id));
            return cards.Select(p => new SimpleCardModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.OriginalImageUrl
            });
        }
        public async Task<IQueryable<SimpleCardModel>> GetCollectionAsync(string username)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(w => w.Username == username);

            if (user == null || user.Collection == null)
            {
                return Enumerable.Empty<SimpleCardModel>().AsQueryable(); 
            }

            var cardIds = user.Collection.Split(',').Select(long.Parse).ToList();

            var cards = await dbContext.Cards.Where(w => cardIds.Contains(w.Id)).ToListAsync();

            return cards.Select(p => new SimpleCardModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.OriginalImageUrl
            }).AsQueryable();
        }
        public async Task AddCardToCollection(string username, long cardIdToAdd)
        {
            using var dbContext = new MyDBContext();
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new ArgumentException($"User with username '{username}' not found.");
            }

          
            var existingCollection = user.Collection != null
                ? user.Collection.Split(',').Select(long.Parse).ToList()
                : new List<long>();
        
            foreach(var card in existingCollection)
            {
                Console.WriteLine(card);
            }
            existingCollection.Add(cardIdToAdd);
         
            user.Collection = string.Join(",", existingCollection);
            Console.WriteLine("Adding card to collection"+user.Collection);
            await dbContext.SaveChangesAsync();
        }
        public async Task RemoveCardFromCollection(string username, long cardIdToRemove)
        {
            using var dbContext = new MyDBContext();

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new ArgumentException($"User with username '{username}' not found.");
            }

            var existingCollection = user.Collection != null
                ? user.Collection.Split(',').Select(long.Parse).ToList()
                : new List<long>();

            if (existingCollection.Contains(cardIdToRemove))
            {
                existingCollection.Remove(cardIdToRemove);

                user.Collection = existingCollection.Count > 0
                    ? string.Join(",", existingCollection)
                    : null; 

                Console.WriteLine("Removing card from collection. New collection: " + user.Collection);

                await dbContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"Card with ID {cardIdToRemove} not found in the collection of user '{username}'.");
               
            }
        }
        public IQueryable<SimpleCardModel> GetCards(SearchParameters search)
        {
            Console.WriteLine(search.ToString());
            IQueryable<Card> cards = dbContext.Cards
                                          .Include(c=>c.RarityCodeNavigation);

            if (!string.IsNullOrEmpty(search.cardName))
            {
                Console.WriteLine("Name Filtering");
                cards = cards.Where(w => w.Name.Contains(search.cardName));
            }

            if (!string.IsNullOrEmpty(search.cardText))
            {
                Console.WriteLine("Text Filtering");
                cards = cards.Where(w => w.Text.Contains(search.cardText));
            }
            // CMC
            if (search.cmc != -1)
            {
                Console.WriteLine("CMC Filtering");
                cards = cards.Where(w => w.ConvertedManaCost == search.cmc.ToString());
            }
            // Rarity Filtering
            if (search.rarities.Count > 0 && search.rarities[0]!="")
            {
                Console.WriteLine("Rarity Filtering");
                cards = cards.Where(card => search.rarities.Any(
                         rarity => card.RarityCodeNavigation.Name == rarity));

            }

            // Color Filtering
            if (search.colors.Count>0 && search.colors[0]!="")
            {
                Console.WriteLine("Color Filtering");
                if (!search.exactColors)
                {
                    cards = cards
                            .Join(dbContext.CardColors, c => c.Id, cc => cc.CardId, (c, cc) =>
                            new { card = c, colorId = cc.ColorId })
                            .Join(dbContext.Colors, c => c.colorId, col => col.Id, (c, col) =>
                            new { c.card,color= col.Name })
                        .Where(w => search.colors.Any(color => w.color == color))
                        .Select(s => s.card);
                }
                else
                {    
                    foreach (string color in search.colors)
                    {
                        cards = cards
                            .Join(dbContext.CardColors, c => c.Id, cc => cc.CardId, (c, cc) =>
                            new { card = c, colorId = cc.ColorId })
                            .Join(dbContext.Colors, c => c.colorId, col => col.Id, (c, col) =>
                            new { c.card, color = col.Name })
                        .Where(w => w.color == color)
                        .Select(s => s.card);
                    }
                    cards = cards.Where(card => !card.CardColors.Any(
                             cardColor => !search.colors.Contains(cardColor.Color.Name)));
                }
            }
            // Type Filtering
            if (search.types.Any())
            {      
                    Console.WriteLine("Type Filtering");
                    foreach (string type in search.types)
                    {
                    
                        string str = type.Substring(0,1).ToUpper() + type.Substring(1);
                    Console.WriteLine(str);
                    cards = cards
                            .Join(dbContext.CardTypes, c => c.Id, ct => ct.CardId, (c, ct) =>
                            new { card = c, typeId = ct.TypeId })
                            .Join(dbContext.Types, c => c.typeId, t => t.Id, (c, t) =>
                            new { c.card, type = t.Name })
                        .Where(w => w.type == str)
                        .Select(s => s.card);
                    }
                

            }

            cards = cards.Where(w=>w.OriginalImageUrl!=null);
            //Crashes when trying to get distinct cards
            //cards = cards.DistinctBy(card => card.Name);
            cards = cards.OrderBy(o => o.Name);
            cards = cards.Skip(search.range * search.page);
            cards = cards.Take(search.range);
            return cards.Select(p => new SimpleCardModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.OriginalImageUrl
            })
              .OrderBy(o => o.Name);
        }

        public IQueryable<SimpleCardModel> GetCardsByName(string search)
        {
            IQueryable<Card> cards = dbContext.Cards;
            cards = cards.Where(w => w.Name==search);
            cards = cards.Where(w => w.OriginalImageUrl != null);
            return cards.Select(p => new SimpleCardModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.OriginalImageUrl,
                Text=p.Text,
                ConvertedManaCost = p.ManaCost,
                SetCode=p.SetCode
            });
        }

        public IQueryable<SimpleCardModel> GetCardById(int search)
        {
            IQueryable<Card> cards = dbContext.Cards;
            cards = cards.Where(w => w.Id == search);
            cards = cards.Where(w => w.OriginalImageUrl != null);
            return cards.Select(p => new SimpleCardModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.OriginalImageUrl,
                Text = p.Text,
                ConvertedManaCost = p.ManaCost,
                SetCode = p.SetCode
            });
        }
        public async Task<SimpleCardModel> GetCardByIdAsync(int search)
        {
            var card = await dbContext.Cards
                            .Where(w => w.Id == search && w.OriginalImageUrl != null)
                            .Select(p => new SimpleCardModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Image = p.OriginalImageUrl,
                                Text = p.Text,
                                ConvertedManaCost = p.ManaCost,
                                SetCode = p.SetCode
                            })
                            .FirstOrDefaultAsync(); 

            return card; 
        }


        public IQueryable GetCardsByColor(string color)
        {
            IQueryable cardsByColor = dbContext.CardColors.Include(c => c.Color)
                .Where(w => w.Color.Name == color)
                .Select(p => new SimpleCardModel
                {
                    Id = p.CardId,
                    Name = p.Card.Name,
                    Image = p.Card.OriginalImageUrl
                })
                .OrderBy(o => o.Name);
            return cardsByColor;
        }

        public IQueryable GetCardsByType(string type, int idRange)
        {
            IQueryable cardsByType = dbContext.CardTypes.Include(c => c.Type)
                .Where(w => w.CardId >= idRange && w.CardId < idRange + 50 && w.Type.Name == type)
                .Select(p => new SimpleCardModel
                {
                    Id = p.CardId,
                    Name = p.Card.Name,
                    Image = p.Card.OriginalImageUrl
                })
                .OrderBy(o => o.Name);
            return cardsByType;
        }
        public class SimpleCardModel()
        {
            public long Id { get; set; }

            public string Name { get; set; } = null!;

            public string? ManaCost { get; set; }

            public string ConvertedManaCost { get; set; } = null!;

            public string Type { get; set; } = null!;

            public string? RarityCode { get; set; }

            public string SetCode { get; set; } = null!;
            public string? Text { get; set; }
            public string Image { get; set; }
        }


    }
}
