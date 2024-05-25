﻿using MTG.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

//using MoreLinq;

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
        public IQueryable<CardModel> GetCollection(string username)
        {
            Console.WriteLine(username);
            string collection = dbContext.Users
                .Where(w => w.Username == username)
                .Select(p => p.Collection)
                .FirstOrDefault();
            Console.WriteLine(collection);
            string[] subs = collection.Split(',');

            foreach (string sub in subs)
            {
                Console.WriteLine(sub);
            }//
            var cardIds = subs.Select(int.Parse).ToList(); // Collect IDs first
            IQueryable<Card> cards = dbContext.Cards.Where(w => cardIds.Contains((int)w.Id));
            return cards.Select(p => new CardModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.OriginalImageUrl
            });
        }

        public IQueryable<CardModel> GetCards(SearchParameters search)
        {
            Console.WriteLine(search.ToString());
            IQueryable<Card> cards = dbContext.Cards
                                          .Include(c=>c.RarityCodeNavigation);

            // Text
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
            return cards.Select(p => new CardModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.OriginalImageUrl
            })
              .OrderBy(o => o.Name);
        }

        public IQueryable<CardModel> GetCardsByName(string search)
        {
            Console.WriteLine(search.ToString());
            IQueryable<Card> cards = dbContext.Cards;
            cards = cards.Where(w => w.Name==search);
            cards = cards.Where(w => w.OriginalImageUrl != null);
            foreach (var item in cards)
            {
                Console.WriteLine($"{item.Name}");
            }
            return cards.Select(p => new CardModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.OriginalImageUrl,
                CardText=p.Text,
                CardCost = p.ManaCost,
                SetCode=p.SetCode
            });
        }

        public IQueryable<CardModel> GetCardsId(int search)
        {
            IQueryable<Card> cards = dbContext.Cards;
            cards = cards.Where(w => w.Id == search);
            cards = cards.Where(w => w.OriginalImageUrl != null);
            return cards.Select(p => new CardModel
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.OriginalImageUrl,
                CardText = p.Text,
                CardCost = p.ManaCost,
                SetCode = p.SetCode
            });
        }


        public IQueryable GetCardsByColor(string color)
        {
            IQueryable cardsByColor = dbContext.CardColors.Include(c => c.Color)
                .Where(w => w.Color.Name == color)
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
            public string? CardText { get; init; }
            public string? CardCost { get; init; }
            public string? Image { get; init; }
            public string? Color { get; init; }
            public string? SetCode { get; init; }

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
