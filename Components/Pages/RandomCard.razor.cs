using MTG.Services;
using static MTG.Services.Search;

namespace MTG.Components.Pages
{
    public partial class RandomCard
    {
        Search services = new Search();
        IQueryable<SimpleCardModel> randomCard;


        protected override void OnInitialized()
        {
            base.OnInitialized();
            int rnd = new Random().Next(1, 5000);
            randomCard = services.GetCardById(rnd);
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
              await GetRandomCard();
            }
        }
        protected async Task GetRandomCard()
        {
            
                
                while (randomCard==null)
                {
                    int rnd = new Random().Next(1, 5000);
                    randomCard = services.GetCardById(rnd);
                    Console.WriteLine("Random card: "+rnd);
                    if(randomCard.First().Image==null) randomCard=null;
                }
            try { NavigationManager.NavigateTo($"/Card/{randomCard.First().Name}"); } 
            catch { 
                Console.WriteLine("Error: Card not found");
                NavigationManager.NavigateTo($"/Card/Counterspell"); }

        }

    }
}
