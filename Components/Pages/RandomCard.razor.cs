using MTG.Services;

namespace MTG.Components.Pages
{
    public partial class RandomCard
    {
        Search services = new Search();
        IQueryable<Models.Card> randomCard;


        protected override void OnInitialized()
        {
            base.OnInitialized();
            int rnd = new Random().Next(1, 5000);
            randomCard = services.GetCardById(rnd);
            Thread.Sleep(1);
            Console.WriteLine("Random card is: " + randomCard.First().Name);
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                NavigationManager.NavigateTo($"/Card/{randomCard.First().Name}");
            }
        }

    }
}
