using Microsoft.AspNetCore.Components;
using MTG.Services;
using System.Linq;
using static MTG.Services.Search;


namespace MTG.Components.Pages
{
    public partial class CardPage
    {
        IQueryable<SimpleCardModel> myCards;
        Services.Search services = new Services.Search();
        [Parameter] public string? searchString { get; set; }
        int cardNumber;
        private SimpleCardModel selectedCard;
        string? userName;
        private void SelectCard(SimpleCardModel card)
        {
            selectedCard = card;
        }
        protected async Task AddCard()
        {
            var result = await ProtectedSessionStorage.GetAsync<string>("User");
            if (result.Success)
            {
                userName = result.Value;
                Console.WriteLine("This is adding card to collection of: "+userName);
            }
            if (userName != null)
            {
               await services.AddCardToCollection(userName, selectedCard.Id);
               Console.WriteLine("added card"+selectedCard.Name+selectedCard.Id+" to "+userName);

            }
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            myCards = services.GetCardsByName(searchString);
            selectedCard = myCards.FirstOrDefault();
        }


    }
}
