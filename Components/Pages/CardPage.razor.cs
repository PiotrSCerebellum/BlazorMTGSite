using Microsoft.AspNetCore.Components;
using MTG.Services;
using System.Linq;

namespace MTG.Components.Pages
{
    public partial class CardPage
    {
        IQueryable<Services.Search.CardModel> myCards;
        Services.Search services = new Services.Search();
        [Parameter] public string? searchString { get; set; }
        int cardNumber;
        private Services.Search.CardModel selectedCard;
        private void SelectCard(Services.Search.CardModel card)
        {
            selectedCard = card;
        }
        protected void AddCard()
        {
            // Add card to user collection
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            myCards = services.GetCardsByName(searchString);
            selectedCard = myCards.FirstOrDefault();
        }


    }
}
