using MTG.Services;
using static MTG.Services.Search;

namespace MTG.Components.Pages
{
    public partial class RandomCard
    {
        Search services = new Search();
        IQueryable<SimpleCardModel> randomCard;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
              await GetRandomCard();
            }
        }
        protected async Task GetRandomCard()
        {
            
                
                while (randomCard == null)
                {
                    int rnd = new Random().Next(1, 5000);
                    randomCard = services.GetCardById(rnd);
                }
            try { NavigationManager.NavigateTo($"/Card/{randomCard.First().Name}"); } 
            catch { NavigationManager.NavigateTo($"/Card/Counterspell"); }

        }

    }
}
